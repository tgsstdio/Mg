using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Magnesium.OpenGL
{
	public class GLSLGraphicsPipelineCompilier : IGLGraphicsPipelineCompiler
	{
		private IGLShaderModuleEntrypoint mShaderModuleEntrypoint;
		private IGLGraphicsPipelineEntrypoint mProgramEntrypoint;
		private IGLErrorHandler mErrHandler;

		public GLSLGraphicsPipelineCompilier(
			IGLShaderModuleEntrypoint shaderModule,
			IGLGraphicsPipelineEntrypoint program,
			IGLErrorHandler errHandler
		)
		{
			mShaderModuleEntrypoint = shaderModule;
			mProgramEntrypoint = program;
			mErrHandler = errHandler;
		}

		public int Compile(MgGraphicsPipelineCreateInfo info)
		{		
			var modules = new List<int>();
			foreach (var stage in info.Stages)
			{
				//var shaderType = ShaderType.VertexShader;
				//if (stage.Stage == MgShaderStageFlagBits.FRAGMENT_BIT)
				//{
				//	shaderType = ShaderType.FragmentShader;
				//}
				//else if (stage.Stage == MgShaderStageFlagBits.VERTEX_BIT)
				//{
				//	shaderType = ShaderType.VertexShader;
				//}
				//else if (stage.Stage == MgShaderStageFlagBits.COMPUTE_BIT)
				//{
				//	shaderType = ShaderType.ComputeShader;
				//}
				//else if (stage.Stage == MgShaderStageFlagBits.GEOMETRY_BIT)
				//{
				//	shaderType = ShaderType.GeometryShader;
				//}
				var module = (GLShaderModule)stage.Module;
				Debug.Assert(module != null);
				if (module.ShaderId.HasValue)
				{
					modules.Add(module.ShaderId.Value);
				}
				else
				{
					using (var ms = new MemoryStream())
					{
						module.Info.Code.CopyTo(ms, (int)module.Info.CodeSize.ToUInt32());
						ms.Seek(0, SeekOrigin.Begin);
						// FIXME : Encoding type 
						using (var sr = new StreamReader(ms))
						{
							string fileContents = sr.ReadToEnd();
							module.ShaderId = CompileShader(mShaderModuleEntrypoint, stage.Stage, fileContents, string.Empty);
							modules.Add(module.ShaderId.Value);
						}
					}
				}
			}
			return LinkShaders(mProgramEntrypoint, mErrHandler, modules.ToArray());
		}

		internal static int LinkShaders(IGLGraphicsPipelineEntrypoint entrypoint, IGLErrorHandler errHandler, int[] shaders)
		{
			int retVal = entrypoint.CreateProgram();
			foreach (var shader in shaders)
			{
				entrypoint.AttachShaderToProgram(retVal, shader);
				//GL.AttachShader (retVal, shader);
			}
			entrypoint.CompileProgram(retVal);

			bool isCompiled = entrypoint.IsCompiled(retVal);
			//int linkStatus = 0;
			//GL.GetProgram(retVal,GetProgramParameterName.LinkStatus, out linkStatus);
			// return (linkStatus == (int)All.True)

			//int glinfoLogLength = 0;
			//GL.GetProgram(retVal, GetProgramParameterName.InfoLogLength, out glinfoLogLength);
			// return (glinfoLogLength > 1)

			bool hasMessages = entrypoint.HasCompilerMessages(retVal);

			if (hasMessages)
			{
				string buffer = entrypoint.GetCompilerMessages(retVal);

					// GL.GetProgramInfoLog(retVal);
				if (!isCompiled)
				{
					errHandler.Trace("Shader Linking failed with the following errors:");
				}
				else {
					errHandler.Trace("Shader Linking succeeded, with following warnings/messages:\n");
				}

				errHandler.Trace(buffer);
			}

			if (!isCompiled)
			{
				//		        #ifndef POSIX
				//		            assert(!"Shader failed linking, here's an assert to break you in the debugger.");
				//		        #endif
				entrypoint.DeleteProgram(retVal);
				retVal = 0;
			}

			return retVal;
		}

		internal static int CompileShader(IGLShaderModuleEntrypoint entrypoint, MgShaderStageFlagBits stage, string fileContents, string shaderPrefix)
		{
			int retVal = entrypoint.CreateShaderModule(stage);
			// GL.CreateShader(type);
			//string includePath = ".";

			// GLSL has this annoying feature that the #version directive must appear first. But we 
			// want to inject some #define shenanigans into the shader. 
			// So to do that, we need to split for the part of the shader up to the end of the #version line,
			// and everything after that. We can then inject our defines right there.
			var strTuple = VersionSplit(fileContents);
			string versionStr = strTuple.Item1;
			string shaderContents = strTuple.Item2;

			var builder = new StringBuilder();
			builder.AppendLine(versionStr);
			builder.AppendLine(shaderPrefix);
			builder.Append(shaderContents);

			entrypoint.CompileShaderModule(retVal, builder.ToString());

			//GL.ShaderSource(retVal, builder.ToString());
			//GL.CompileShader(retVal);

			bool isCompiled = entrypoint.IsCompiled(retVal);

			//int compileStatus = 0;
			//GL.GetShader(retVal, ShaderParameter.CompileStatus, out compileStatus);
			//// if (compileStatus != (int)All.True)

			//int glinfoLogLength = 0;
			//GL.GetShader(retVal, ShaderParameter.InfoLogLength, out glinfoLogLength);
			//if (glinfoLogLength > 1)

			bool hasMessages = entrypoint.HasCompilerMessages(retVal);

			if (hasMessages)
			{
				string buffer = entrypoint.GetCompilerMessages(retVal);
					//	GL.GetShaderInfoLog(retVal);
				if (!isCompiled)
				{
					throw new Exception("Shader Compilation failed for shader with the following errors: " + buffer);
				}
				//				else {
				//					Console.WriteLine("Shader Compilation succeeded for shader '{0}', with the following log:", _shaderFilename);
				//				}
				//
				//				Console.WriteLine(buffer);
			}

			if (!isCompiled)
			{
				entrypoint.DeleteShaderModule(retVal);
				retVal = 0;
			}

			return retVal;
		}

		private static Tuple<string, string> VersionSplit(string srcString)
		{
			int length = srcString.Length;
			int substrStartPos = 0;
			int eolPos = 0;
			for (eolPos = substrStartPos; eolPos < length; ++eolPos)
			{
				if (srcString[eolPos] != '\n')
				{
					continue;
				}

				if (MatchVersionLine(srcString, substrStartPos, eolPos + 1))
				{
					return DivideString(srcString, eolPos + 1);
				}

				substrStartPos = eolPos + 1;
			}

			// Could be on the last line (not really--the shader will be invalid--but we'll do it anyways)
			if (MatchVersionLine(srcString, substrStartPos, length))
			{
				return DivideString(srcString, eolPos + 1);
			}

			return new Tuple<string, string>("", srcString);
		}

		private static bool MatchVersionLine(string srcString, int startPos, int endPos)
		{
			int checkPos = startPos;
			//Assert(_endPos <= _srcString.Length);

			// GCC doesn't support regexps yet, so we're doing a hand-coded look for 
			// ^\s*#\s*version\s+\d+\s*$
			// Annoying!

			// ^ was handled by the caller.

			// \s*
			while (checkPos < endPos && (srcString[checkPos] == ' ' || srcString[checkPos] == '\t'))
			{
				++checkPos;
			}

			if (checkPos == endPos)
			{
				return false;
			}

			// #
			if (srcString[checkPos] == '#')
			{
				++checkPos;
			}
			else {
				return false;
			}

			if (checkPos == endPos)
			{
				return false;
			}

			// \s*
			while (checkPos < endPos && (srcString[checkPos] == ' ' || srcString[checkPos] == '\t'))
			{
				++checkPos;
			}

			if (checkPos == endPos)
			{
				return false;
			}

			// version
			const string kSearchString = "version";
			int kSearchStringLen = kSearchString.Length;

			if (checkPos + kSearchStringLen >= endPos)
			{
				return false;
			}

			if (string.Compare(kSearchString, 0, srcString, checkPos, kSearchStringLen, StringComparison.Ordinal) == 0)
			{
				checkPos += kSearchStringLen;
			}
			else {
				return false;
			}

			// \s+ (as \s\s*)
			if (srcString[checkPos] == ' ' || srcString[checkPos] == '\t')
			{
				++checkPos;
			}
			else {
				return false;
			}

			while (checkPos < endPos && (srcString[checkPos] == ' ' || srcString[checkPos] == '\t'))
			{
				++checkPos;
			}

			if (checkPos == endPos)
			{
				return false;
			}

			// \d+ (as \d\d*)
			if (srcString[checkPos] >= '0' && srcString[checkPos] <= '9')
			{
				++checkPos;
			}
			else {
				return false;
			}

			// Check the version number
			while (checkPos < endPos && (srcString[checkPos] >= '0' && srcString[checkPos] <= '9'))
			{
				++checkPos;
			}

			while (checkPos < endPos && (srcString[checkPos] == ' ' || srcString[checkPos] == '\t'))
			{
				++checkPos;
			}

			while (checkPos < endPos && (srcString[checkPos] == '\r' || srcString[checkPos] == '\n'))
			{
				++checkPos;
			}

			// NOTE that if the string terminates here we're successful (unlike above)
			if (checkPos == endPos)
			{
				return true;
			}

			return false;
		}

		private static Tuple<string, string> DivideString(string srcString, int splitEndPos)
		{
			return new Tuple<string, string>(
				srcString.Substring(0, splitEndPos),
				srcString.Substring(splitEndPos));
		}
	}
}

