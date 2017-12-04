using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL
{
    public interface IGLCmdStateRenderer
    {
        // TODO : hide these implementation details
        void Initialize();
        void BeginRenderpass(GLCmdSubpassOperation record);
        void EndRenderpass();
        void BindPipeline(GLCmdBoundPipelineRecordInfo pipelineInfo);
        void UpdateStencilWriteMask(GLCmdPipelineStencilWriteInfo write);
        void UpdateViewports(GLCmdViewportParameter viewports);
        void UpdateScissors(GLCmdScissorParameter scissors);
        void UpdateDepthBounds(GLCmdDepthBoundsParameter bounds);
        void UpdateBlendConstants(MgColor4f blendConstants);
        void UpdateDepthBias(GLCmdDepthBiasParameter nextDepthBias);
        void UpdateLineWidth(float lineWidth);
        void UpdateFrontStencil(GLCmdStencilFunctionInfo stencilInfo);
        void UpdateBackStencil(GLCmdStencilFunctionInfo stencilInfo);

        void Draw(GLCmdInternalDraw drawItem);
        void DrawIndexed(GLCmdInternalDrawIndexed drawItem);
        void DrawIndexedIndirect(GLCmdInternalDrawIndexedIndirect drawItem);
        void DrawIndirect(GLCmdInternalDrawIndirect drawItem);
        void BindVertexArrays(GLCmdVertexBufferObject vao);
        void BindDescriptorSets(GLCmdDescriptorSetParameter ds);
    }
}