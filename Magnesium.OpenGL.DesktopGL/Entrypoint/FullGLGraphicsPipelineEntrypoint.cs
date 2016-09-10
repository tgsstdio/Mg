using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLGraphicsPipelineEntrypoint : IGLGraphicsPipelineEntrypoint
	{
		private IGLErrorHandler mErrHandler;
		public FullGLGraphicsPipelineEntrypoint(IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
		}

		public void AttachShaderToProgram(int programID, int shader)
		{
			GL.AttachShader (programID, shader);
		}

		public void CompileProgram(int programID)
		{
			GL.LinkProgram(programID);
		}

		public int CreateProgram()
		{
			return GL.CreateProgram();
		}

		public void DeleteProgram(int programID)
		{
			GL.DeleteProgram(programID);
		}

		public bool HasCompilerMessages(int programID)
		{
			int glinfoLogLength = 0;
			GL.GetProgram(programID, GetProgramParameterName.InfoLogLength, out glinfoLogLength);
			return (glinfoLogLength > 1);
		}

		public bool CheckUniformLocation(int programId, int location)
		{
			int locationQuery;
			GL.Ext.GetUniform(programId, location, out locationQuery);
			mErrHandler.LogGLError("FullGLShaderModuleEntrypoint.CheckUniformLocation");
			return (locationQuery != -1);
		}

		public int GetActiveUniforms(int programId)
		{
			int noOfActiveUniforms;
			GL.GetProgram(programId, GetProgramParameterName.ActiveUniforms, out noOfActiveUniforms);
			mErrHandler.LogGLError("FullGLShaderModuleEntrypoint.GetActiveUniforms");
			return noOfActiveUniforms;
		}

		public string GetCompilerMessages(int programID)
		{
			return GL.GetProgramInfoLog(programID);
		}

		public bool IsCompiled(int programID)
		{
			int linkStatus = 0;
			GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out linkStatus);
			return (linkStatus == (int)All.True);
		}
	}
}

