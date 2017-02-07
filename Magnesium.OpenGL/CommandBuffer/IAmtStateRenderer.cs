﻿namespace Magnesium.OpenGL.Internals
{
    public interface IAmtStateRenderer
    {
        // TODO : hide these implementation details
        void Initialize();
        void BeginRenderpass(AmtBeginRenderpassRecord record);
        void EndRenderpass();
        void BindPipeline(AmtBoundPipelineRecordInfo pipelineInfo);
        void UpdateStencilWriteMask(AmtPipelineStencilWriteInfo write);
        void UpdateViewports(GLCmdViewportParameter viewports);
        void UpdateScissors(GLCmdScissorParameter scissors);
        void UpdateDepthBounds(GLCmdDepthBoundsParameter bounds);
        void UpdateBlendConstants(MgColor4f blendConstants);
        void UpdateDepthBias(GLCmdDepthBiasParameter nextDepthBias);
        void UpdateLineWidth(float lineWidth);
        void UpdateFrontStencil(AmtGLStencilFunctionInfo stencilInfo);
        void UpdateBackStencil(AmtGLStencilFunctionInfo stencilInfo);

        void Draw(GLCmdInternalDraw drawItem);
        void DrawIndexed(GLCmdInternalDrawIndexed drawItem);
        void DrawIndexedIndirect(GLCmdInternalDrawIndexedIndirect drawItem);
        void DrawIndirect(GLCmdInternalDrawIndirect drawItem);
        void BindVertexArrays(GLCmdVertexBufferObject vao);
    }
}