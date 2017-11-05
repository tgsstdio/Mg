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
            mErrHandler.LogGLError("AttachShaderToProgram");
        }

        public void CompileProgram(int programID)
        {
            GL.LinkProgram(programID);
            mErrHandler.LogGLError("CompileProgram");
        }

        public int CreateProgram()
        {
            var program = GL.CreateProgram();
            mErrHandler.LogGLError("CreateProgram");
            return program;
        }

        public void DeleteProgram(int programID)
        {
            GL.DeleteProgram(programID);
            mErrHandler.LogGLError("DeleteProgram");
        }

        public bool HasCompilerMessages(int programID)
        {
            int glinfoLogLength = 0;
            GL.GetProgram(programID, GetProgramParameterName.InfoLogLength, out glinfoLogLength);
            mErrHandler.LogGLError("HasCompilerMessages");
            return (glinfoLogLength > 1);
        }

        public bool CheckUniformLocation(int programId, int location)
        {
            int locationQuery = -1;

            string name = GL.GetActiveUniformName(programId, location);

            GL.Ext.GetUniform(programId, location, out locationQuery);
            mErrHandler.LogGLError("CheckUniformLocation");
            return (locationQuery != -1);
        }

        public int GetActiveUniforms(int programId)
        {
            int noOfActiveUniforms = 0;
            GL.GetProgram(programId, GetProgramParameterName.ActiveUniforms, out noOfActiveUniforms);
            mErrHandler.LogGLError("GetActiveUniforms");
            return noOfActiveUniforms;
        }

        public string GetCompilerMessages(int programID)
        {
            var message = GL.GetProgramInfoLog(programID);
            mErrHandler.LogGLError("GetCompilerMessages");
            return message;
        }

        public bool IsCompiled(int programID)
        {
            int linkStatus = 0;
            GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out linkStatus);
            mErrHandler.LogGLError("IsCompiled");
            return (linkStatus == (int)All.True);
        }

        public string[] GetUniformBlocks(int programID)
        {
            GL.GetProgram(programID, GetProgramParameterName.ActiveUniformBlocks, out int noOfUniformBlocks);
            mErrHandler.LogGLError("GetUniformBlocks.GetProgram");

            GL.GetInteger(GetPName.MaxUniformBufferBindings, out int noOfUniformBlockBindings);
            mErrHandler.LogGLError("GetUniformBlocks.MaxUniformBufferBindings");

            var names = new string[noOfUniformBlocks];
	        for (int i = 0; i < noOfUniformBlocks; i += 1)
            {
                names[i] = GL.GetActiveUniformBlockName(programID, i);
                mErrHandler.LogGLError("GetUniformBlocks.GetActiveUniformBlockName");
                var index = GL.GetUniformBlockIndex(programID, names[i]);
                mErrHandler.LogGLError("GetUniformBlocks.GetUniformBlockIndex");

                int length;
                var queries = new ProgramProperty[] {
                    ProgramProperty.BufferBinding,
                    ProgramProperty.ReferencedByFragmentShader,
                    ProgramProperty.ReferencedByVertexShader,
                    ProgramProperty.BufferDataSize
                };
                var props = new int[queries.Length];
                GL.GetProgramResource(programID, ProgramInterface.UniformBlock, i, queries.Length, queries, props.Length, out length, props);
                mErrHandler.LogGLError("GetUniformBlocks.GetProgramResource");
            }
            return names;
        }
    }
}

