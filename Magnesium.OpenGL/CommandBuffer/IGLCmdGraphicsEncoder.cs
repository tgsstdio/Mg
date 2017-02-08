namespace Magnesium.OpenGL.Internals
{
    public interface IGLCmdGraphicsEncoder
    {
        GLCmdGraphicsGrid AsGrid();
        void Clear();

        void BindPipeline(IMgPipeline pipeline);
        void SetViewports(uint firstViewport, MgViewport[] pViewports);
        void SetScissor(uint firstScissor, MgRect2D[] pScissors);
        void SetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);
        void SetBlendConstants(MgColor4f blendConstants);
        void SetStencilCompareMask(MgStencilFaceFlagBits faceMask, uint compareMask);
        void SetStencilWriteMask(MgStencilFaceFlagBits faceMask, uint writeMask);
        void SetStencilReference(MgStencilFaceFlagBits faceMask, uint reference);
        void BeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents);
        void EndRenderPass();
        void NextSubpass(MgSubpassContents contents);

        void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance);
        void BindIndexBuffer(IMgBuffer buffer, ulong offset, MgIndexType indexType);
        void DrawIndexedIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride);
        void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance);
        void DrawIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride);
        void BindVertexBuffers(uint firstBinding, IMgBuffer[] pBuffers, ulong[] pOffsets);
        void BindDescriptorSets(IMgPipelineLayout layout, uint firstSet, uint descriptorSetCount, IMgDescriptorSet[] pDescriptorSets, uint[] pDynamicOffsets);
        void SetDepthBounds(float minDepthBounds, float maxDepthBounds);
        void SetLineWidth(float lineWidth);
    }
}