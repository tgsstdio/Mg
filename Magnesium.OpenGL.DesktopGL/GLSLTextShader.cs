﻿using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	internal class GLSLTextShader
	{
		readonly IGLGraphicsPipelineEntrypoint mEntrypoint;
		public GLSLTextShader(IGLGraphicsPipelineEntrypoint program)
		{
			mEntrypoint = program;
		}

		internal static int CompileShader(ShaderType type, string fileContents, string shaderPrefix)
		{			
			int retVal = GL.CreateShader(type);
			//string includePath = ".";

			// GLSL has this annoying feature that the #version directive must appear first. But we 
			// want to inject some #define shenanigans into the shader. 
			// So to do that, we need to split for the part of the shader up to the end of the #version line,
			// and everything after that. We can then inject our defines right there.
			var strTuple = VersionSplit(fileContents);
			string versionStr = strTuple.Item1;
			string shaderContents = strTuple.Item2;

			var builder = new StringBuilder ();
			builder.AppendLine (versionStr);
			builder.AppendLine (shaderPrefix);
			builder.Append (shaderContents);

			GL.ShaderSource(retVal, builder.ToString());
			GL.CompileShader(retVal);

			int compileStatus = 0;
			GL.GetShader(retVal, ShaderParameter.CompileStatus, out compileStatus);

			int glinfoLogLength = 0;
			GL.GetShader(retVal, ShaderParameter.InfoLogLength, out glinfoLogLength);
			if (glinfoLogLength > 1) {
				string buffer = GL.GetShaderInfoLog(retVal);
				if (compileStatus != (int) All.True) {
 					throw new Exception("Shader Compilation failed for shader with the following errors: " + buffer);
				}
			}

			if (compileStatus != (int) All.True) 
			{
				GL.DeleteShader(retVal);
				retVal = 0;
			}

			return retVal;
		}

		private static Tuple<string, string> VersionSplit(string srcString)
		{
			int length = srcString.Length;
			int substrStartPos = 0;
			int eolPos = 0;
			for (eolPos = substrStartPos; eolPos < length; ++eolPos) {
				if (srcString[eolPos] != '\n') {
					continue;
				}

				if (MatchVersionLine(srcString, substrStartPos, eolPos + 1)) {
					return DivideString(srcString, eolPos + 1);
				}

				substrStartPos = eolPos + 1;
			}

			// Could be on the last line (not really--the shader will be invalid--but we'll do it anyways)
			if (MatchVersionLine(srcString, substrStartPos, length)) {
				return DivideString(srcString, eolPos + 1);
			}

			return new Tuple<string, string>("", srcString);
		}

		private static Tuple<string, string> DivideString(string srcString, int splitEndPos)
		{
			return new Tuple<string, string>(
				srcString.Substring(0, splitEndPos),
				srcString.Substring(splitEndPos));
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
			while (checkPos < endPos && (srcString[checkPos] == ' ' || srcString[checkPos] == '\t')) {
				++checkPos;
			}

			if (checkPos == endPos) {
				return false;
			}

			// #
			if (srcString[checkPos] == '#') {
				++checkPos;        
			} else {
				return false;
			}

			if (checkPos == endPos) {
				return false;
			}

			// \s*
			while (checkPos < endPos && (srcString[checkPos] == ' ' || srcString[checkPos] == '\t')) {
				++checkPos;
			}

			if (checkPos == endPos) {
				return false;
			}

			// version
			const string kSearchString = "version";
			int kSearchStringLen = kSearchString.Length;

			if (checkPos + kSearchStringLen >= endPos) {
				return false;
			}

			if (string.Compare (kSearchString, 0, srcString, checkPos, kSearchStringLen, StringComparison.Ordinal) == 0) {
				checkPos += kSearchStringLen;
			} else {
				return false;
			}

			// \s+ (as \s\s*)
			if (srcString[checkPos] == ' ' || srcString[checkPos] == '\t') {
				++checkPos;
			} else {
				return false;
			}

			while (checkPos < endPos && (srcString[checkPos] == ' ' || srcString[checkPos] == '\t')) {
				++checkPos;
			}

			if (checkPos == endPos) {
				return false;
			}

			// \d+ (as \d\d*)
			if (srcString[checkPos] >= '0' && srcString[checkPos] <= '9') {
				++checkPos;
			} else {
				return false;
			}

			// Check the version number
			while (checkPos < endPos && (srcString[checkPos] >= '0' && srcString[checkPos] <= '9')) {
				++checkPos;
			}

			while (checkPos < endPos && (srcString[checkPos] == ' ' || srcString[checkPos] == '\t')) {
				++checkPos;
			}

			while (checkPos < endPos && (srcString[checkPos] == '\r' || srcString[checkPos] == '\n')) {
				++checkPos;
			}

			// NOTE that if the string terminates here we're successful (unlike above)
			if (checkPos == endPos) {
				return true;
			}

			return false;
		}

		internal int LinkShaders(params int[] shaders)
		{
			int retVal = mEntrypoint.CreateProgram ();
			foreach (var shader in shaders)
			{
				mEntrypoint.AttachShaderToProgram(retVal, shader);
			}
			mEntrypoint.CompileProgram(retVal);

			bool isLinked = mEntrypoint.IsCompiled(retVal);

			bool hasMessages = mEntrypoint.HasCompilerMessages(retVal);

			if (hasMessages) {
				string buffer = GL.GetProgramInfoLog(retVal);
				if (!isLinked) {
					Console.WriteLine("Shader Linking failed with the following errors:");
				}
				else {
					Console.WriteLine("Shader Linking succeeded, with following warnings/messages:\n");
				}

				Console.WriteLine(buffer);
			}

			if (!isLinked) {
				mEntrypoint.DeleteProgram(retVal);
				retVal = 0;
			}

			return retVal;
		}
	}
}

