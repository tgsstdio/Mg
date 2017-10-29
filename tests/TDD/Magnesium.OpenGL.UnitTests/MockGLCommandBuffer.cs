using Magnesium.OpenGL.Internals;
using System;

namespace Magnesium.OpenGL.UnitTests
{
    class MockGLCommandBuffer : IMgCommandBuffer
    {
        public Result BeginCommandBuffer(MgCommandBufferBeginInfo pBeginInfo)
        {
            throw new NotImplementedException();
        }

        public void CmdBeginQuery(IMgQueryPool queryPool, uint query, MgQueryControlFlagBits flags)
        {
            throw new NotImplementedException();
        }

        public void CmdBeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
        {
            throw new NotImplementedException();
        }

		private IGLDescriptorSetBinder mBinder;
        public void CmdBindDescriptorSets(MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, uint firstSet, IMgDescriptorSet[] pDescriptorSets, uint[] pDynamicOffsets)
        {
			mBinder.Bind(pipelineBindPoint, layout, firstSet, (uint) pDescriptorSets.Length, pDescriptorSets, pDynamicOffsets);
        }

        public void CmdBindIndexBuffer(IMgBuffer buffer, ulong offset, MgIndexType indexType)
        {
            throw new NotImplementedException();
        }

        public void CmdBindPipeline(MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
        {
            throw new NotImplementedException();
        }

        public void CmdBindVertexBuffers(uint firstBinding, IMgBuffer[] pBuffers, ulong[] pOffsets)
        {
            throw new NotImplementedException();
        }

        public void CmdBlitImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter)
        {
            throw new NotImplementedException();
        }

        public void CmdClearAttachments(MgClearAttachment[] pAttachments, MgClearRect[] pRects)
        {
            throw new NotImplementedException();
        }

        public void CmdClearColorImage(IMgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges)
        {
            throw new NotImplementedException();
        }

        public void CmdClearDepthStencilImage(IMgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges)
        {
            throw new NotImplementedException();
        }

        public void CmdCopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
        {
            throw new NotImplementedException();
        }

        public void CmdCopyBufferToImage(IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
        {
            throw new NotImplementedException();
        }

        public void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
        {
            throw new NotImplementedException();
        }

        public void CmdCopyImageToBuffer(IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
        {
            throw new NotImplementedException();
        }

        public void CmdCopyQueryPoolResults(IMgQueryPool queryPool, uint firstQuery, uint queryCount, IMgBuffer dstBuffer, ulong dstOffset, ulong stride, MgQueryResultFlagBits flags)
        {
            throw new NotImplementedException();
        }

        public void CmdDispatch(uint x, uint y, uint z)
        {
            throw new NotImplementedException();
        }

        public void CmdDispatchIndirect(IMgBuffer buffer, ulong offset)
        {
            throw new NotImplementedException();
        }

        public void CmdDraw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
        {
            throw new NotImplementedException();
        }

        public void CmdDrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
        {
            throw new NotImplementedException();
        }

        public void CmdDrawIndexedIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
        {
            throw new NotImplementedException();
        }

        public void CmdDrawIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
        {
            throw new NotImplementedException();
        }

        public void CmdEndQuery(IMgQueryPool queryPool, uint query)
        {
            throw new NotImplementedException();
        }

        public void CmdEndRenderPass()
        {
            throw new NotImplementedException();
        }

        public void CmdExecuteCommands(IMgCommandBuffer[] pCommandBuffers)
        {
            throw new NotImplementedException();
        }

        public void CmdFillBuffer(IMgBuffer dstBuffer, ulong dstOffset, ulong size, uint data)
        {
            throw new NotImplementedException();
        }

        public void CmdNextSubpass(MgSubpassContents contents)
        {
            throw new NotImplementedException();
        }

        public void CmdPipelineBarrier(MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        public void CmdPushConstants(IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, uint offset, uint size, IntPtr pValues)
        {
            throw new NotImplementedException();
        }

        public void CmdResetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask)
        {
            throw new NotImplementedException();
        }

        public void CmdResetQueryPool(IMgQueryPool queryPool, uint firstQuery, uint queryCount)
        {
            throw new NotImplementedException();
        }

        public void CmdResolveImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions)
        {
            throw new NotImplementedException();
        }

        public void CmdSetBlendConstants(MgColor4f blendConstants)
        {
            throw new NotImplementedException();
        }

        public void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
        {
            throw new NotImplementedException();
        }

        public void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds)
        {
            throw new NotImplementedException();
        }

        public void CmdSetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask)
        {
            throw new NotImplementedException();
        }

        public void CmdSetLineWidth(float lineWidth)
        {
            throw new NotImplementedException();
        }

        public void CmdSetScissor(uint firstScissor, MgRect2D[] pScissors)
        {
            throw new NotImplementedException();
        }

        public void CmdSetStencilCompareMask(MgStencilFaceFlagBits faceMask, uint compareMask)
        {
            throw new NotImplementedException();
        }

        public void CmdSetStencilReference(MgStencilFaceFlagBits faceMask, uint reference)
        {
            throw new NotImplementedException();
        }

        public void CmdSetStencilWriteMask(MgStencilFaceFlagBits faceMask, uint writeMask)
        {
            throw new NotImplementedException();
        }

        public void CmdSetViewport(uint firstViewport, MgViewport[] pViewports)
        {
            throw new NotImplementedException();
        }

        public void CmdUpdateBuffer(IMgBuffer dstBuffer, ulong dstOffset, ulong dataSize, IntPtr pData)
        {
            throw new NotImplementedException();
        }

        public void CmdWaitEvents(IMgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        public void CmdWriteTimestamp(MgPipelineStageFlagBits pipelineStage, IMgQueryPool queryPool, uint query)
        {
            throw new NotImplementedException();
        }

        public Result EndCommandBuffer()
        {
            throw new NotImplementedException();
        }

        public Result ResetCommandBuffer(MgCommandBufferResetFlagBits flags)
        {
            throw new NotImplementedException();
        }
    }
}
