using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLUniformBlockEntrypoint : IGLUniformBlockEntrypoint
    {
        private readonly IGLErrorHandler mErrHandler;
        public FullGLUniformBlockEntrypoint(IGLErrorHandler errHandler)
        {
            mErrHandler = errHandler;
        }

        public GLActiveUniformBlockInfo GetActiveUniformBlockInfo(int programId, int activeIndex)
        {
            int length;
            var queries = new ProgramProperty[] {
                    ProgramProperty.BufferBinding,
                    ProgramProperty.BufferDataSize,
                    ProgramProperty.ReferencedByFragmentShader,
                    ProgramProperty.ReferencedByVertexShader,
                    ProgramProperty.ReferencedByGeometryShader,
                    ProgramProperty.ReferencedByTessControlShader,
                    ProgramProperty.ReferencedByTessEvaluationShader,                    
                };
            var props = new int[queries.Length];
            GL.GetProgramResource(programId, ProgramInterface.UniformBlock, activeIndex, queries.Length, queries, props.Length, out length, props);
            mErrHandler.LogGLError("GetActiveUniformBlockInfo");

            int stageIndex = 2;
            MgShaderStageFlagBits stage = props[stageIndex] != 0 ? (MgShaderStageFlagBits.FRAGMENT_BIT) : 0;

            stageIndex += 1;
            stage |= props[stageIndex] != 0 ? (MgShaderStageFlagBits.VERTEX_BIT) : 0;

            stageIndex += 1;
            stage |= props[stageIndex] != 0 ? (MgShaderStageFlagBits.GEOMETRY_BIT) : 0;

            stageIndex += 1;
            stage |= props[stageIndex] != 0 ? (MgShaderStageFlagBits.TESSELLATION_CONTROL_BIT) : 0;

            stageIndex += 1;
            stage |= props[stageIndex] != 0 ? (MgShaderStageFlagBits.TESSELLATION_EVALUATION_BIT) : 0;


            return new GLActiveUniformBlockInfo
            {
                BindingIndex = props[0],
                Stride = props[1],             
                Stage = stage,
            };
        }

        public string GetActiveUniformBlockName(int programId, int activeIndex)
        {
            var name = GL.GetActiveUniformBlockName(programId, activeIndex);
            mErrHandler.LogGLError("GetActiveUniformBlockName");
            return name;
        }

        public int GetNoOfActiveUniformBlocks(int programId)
        {
            GL.GetProgram(programId, GetProgramParameterName.ActiveUniformBlocks, out int noOfUniformBlocks);
            mErrHandler.LogGLError("GetNoOfActiveUniformBlocks");
            return noOfUniformBlocks;
        }
    }
}
