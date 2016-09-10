using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLShaderModuleEntrypoint : IGLShaderModuleEntrypoint
	{
		#region IGLShaderModuleEntrypoint implementation

		IGLErrorHandler mErrHandler;

		public FullGLShaderModuleEntrypoint (IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
		}

		//public int CompileProgram (MgGraphicsPipelineCreateInfo info)
		//{
		//	var modules = new List<int> ();
		//	foreach (var stage in info.Stages)
		//	{
		//		var shaderType = ShaderType.VertexShader;
		//		if (stage.Stage == MgShaderStageFlagBits.FRAGMENT_BIT)
		//		{
		//			shaderType = ShaderType.FragmentShader;
		//		}
		//		else if (stage.Stage == MgShaderStageFlagBits.VERTEX_BIT)
		//		{
		//			shaderType = ShaderType.VertexShader;
		//		}
		//		else if (stage.Stage == MgShaderStageFlagBits.COMPUTE_BIT)
		//		{
		//			shaderType = ShaderType.ComputeShader;
		//		}
		//		else if (stage.Stage == MgShaderStageFlagBits.GEOMETRY_BIT)
		//		{
		//			shaderType = ShaderType.GeometryShader;
		//		}
		//		var module = (GLShaderModule) stage.Module;
		//		Debug.Assert(module != null);
		//		if (module.ShaderId.HasValue)
		//		{
		//			modules.Add (module.ShaderId.Value);
		//		}
		//		else
		//		{
		//			using (var ms = new MemoryStream ())
		//			{
		//				module.Info.Code.CopyTo (ms, (int)module.Info.CodeSize.ToUInt32 ());
		//				ms.Seek (0, SeekOrigin.Begin);
		//				// FIXME : Encoding type 
		//				using (var sr = new StreamReader (ms))
		//				{
		//					string fileContents = sr.ReadToEnd ();
		//					module.ShaderId = GLSLTextShader.CompileShader (shaderType, fileContents, string.Empty);
		//					modules.Add (module.ShaderId.Value);
		//				}
		//			}
		//		}
		//	}
		//	return GLSLTextShader.LinkShaders (modules.ToArray ());
		//}

		public void DeleteShaderModule (int module)
		{
			GL.DeleteShader(module);
		}

		public int CreateShaderModule(MgShaderStageFlagBits stage)
		{
			var shaderType = ShaderType.VertexShader;
			switch (stage)
			{
				case MgShaderStageFlagBits.FRAGMENT_BIT:
					shaderType = ShaderType.FragmentShader;
					break;
				case MgShaderStageFlagBits.VERTEX_BIT:
					shaderType = ShaderType.VertexShader;
					break;
				case MgShaderStageFlagBits.COMPUTE_BIT:
					shaderType = ShaderType.ComputeShader;
					break;
				case MgShaderStageFlagBits.GEOMETRY_BIT:
					shaderType = ShaderType.GeometryShader;
					break;
				default:
					throw new NotSupportedException();
			}

			return GL.CreateShader(shaderType);
		}

		public void CompileShaderModule(int module, string sourceCode)
		{
			GL.ShaderSource(module, sourceCode);
			GL.CompileShader(module);
		}

		public bool HasCompilerMessages(int module)
		{
			int glinfoLogLength = 0;
			GL.GetShader(module, ShaderParameter.InfoLogLength, out glinfoLogLength);
			return (glinfoLogLength > 1);
		}

		public bool IsCompiled(int module)
		{
			int compileStatus = 0;
			GL.GetShader(module, ShaderParameter.CompileStatus, out compileStatus);
			return compileStatus == (int)All.True;
		}

		public string GetCompilerMessages(int module)
		{
			return 	GL.GetShaderInfoLog(module);
		}

		#endregion
	}
}

