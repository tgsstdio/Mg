using System;
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

		public void DeleteShaderModule (int module)
		{
			GL.DeleteShader(module);
            mErrHandler.LogGLError("DeleteShaderModule");
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

			var result = GL.CreateShader(shaderType);
            mErrHandler.LogGLError("CreateShaderModule");
            return result;
        }

		public void CompileShaderModule(int module, string sourceCode)
		{
			GL.ShaderSource(module, sourceCode);
            mErrHandler.LogGLError("CompileShaderModule.ShaderSource");
            GL.CompileShader(module);
            mErrHandler.LogGLError("CompileShaderModule.CompileShader");
        }

		public bool HasCompilerMessages(int module)
		{
            GL.GetShader(module, ShaderParameter.InfoLogLength, out int glinfoLogLength);
            mErrHandler.LogGLError("HasCompilerMessages");
            return (glinfoLogLength > 1);
		}

		public bool IsCompiled(int module)
		{
            GL.GetShader(module, ShaderParameter.CompileStatus, out int compileStatus);
            mErrHandler.LogGLError("IsCompiled");
            return compileStatus == (int)All.True;
		}

		public string GetCompilerMessages(int module)
		{
			var message = GL.GetShaderInfoLog(module);
            mErrHandler.LogGLError("GetCompilerMessages");
            return message;
        }

		#endregion
	}
}

