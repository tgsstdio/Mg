namespace Magnesium.OpenGL
{
    public interface IAmtStateRenderer
    {
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

        void Draw(GLCmdBufferDrawItem drawItem);
        void DrawIndexed(GLCmdBufferDrawItem drawItem);
        void DrawIndexedIndirect(GLCmdBufferDrawItem drawItem);
        void DrawIndirect(GLCmdBufferDrawItem drawItem);
        void BindVertexArrays(object vao);
    }
}