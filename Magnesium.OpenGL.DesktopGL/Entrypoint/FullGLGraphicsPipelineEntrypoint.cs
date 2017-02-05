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
            GL.AttachShader(programID, shader);
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

        public bool CheckUniformLocation(int programId, uint location)
        {
            int locationQuery;
            GL.Ext.GetUniform(programId, (int) location, out locationQuery);
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

        public string[] GetUniformBlocks(int programID)
        {
            int noOfUniformBlocks;
            GL.GetProgram(programID, GetProgramParameterName.ActiveUniformBlocks, out noOfUniformBlocks);

            int noOfUniformBlockBindings;
            GL.GetInteger(GetPName.MaxUniformBufferBindings, out noOfUniformBlockBindings);

            var names = new string[noOfUniformBlocks];
	        for (int i = 0; i < noOfUniformBlocks; i += 1)
            {
                names[i] = GL.GetActiveUniformBlockName(programID, i);
                var index = GL.GetUniformBlockIndex(programID, names[i]);

                int length;
                var queries = new ProgramProperty[] {
                    ProgramProperty.BufferBinding,
                    ProgramProperty.ReferencedByFragmentShader,
                    ProgramProperty.ReferencedByVertexShader,
                    ProgramProperty.BufferDataSize
                };
                var props = new int[queries.Length];
                GL.GetProgramResource(programID, ProgramInterface.UniformBlock, i, queries.Length, queries, props.Length, out length, props);
            }
            return names;
        }
    }
}

