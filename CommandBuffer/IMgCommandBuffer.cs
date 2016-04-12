using System;

namespace Magnesium
{
    // CommandBuffer
    public interface IMgCommandBuffer
	{
		Result BeginCommandBuffer(MgCommandBufferBeginInfo pBeginInfo);
		Result EndCommandBuffer();
		Result ResetCommandBuffer(MgCommandBufferResetFlagBits flags);
		void CmdBindPipeline(MgPipelineBindPoint pipelineBindPoint, MgPipeline pipeline);
		void CmdSetViewport(UInt32 firstViewport, UInt32 viewportCount, MgViewport[] pViewports);
		void CmdSetScissor(UInt32 firstScissor, UInt32 scissorCount, MgRect2D[] pScissors);
		void CmdSetLineWidth(float lineWidth);
		void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);
		void CmdSetBlendConstants(float[] blendConstants); // 4
		void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds);
		void CmdSetStencilCompareMask(MgStencilFaceFlagBits faceMask, UInt32 compareMask);
		void CmdSetStencilWriteMask(MgStencilFaceFlagBits faceMask, UInt32 writeMask);
		void CmdSetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference);
		void CmdBindDescriptorSets(MgPipelineBindPoint pipelineBindPoint, MgPipelineLayout layout, UInt32 firstSet, UInt32 descriptorSetCount, MgDescriptorSet[] pDescriptorSets, UInt32[] pDynamicOffsets);
		void CmdBindIndexBuffer(MgBuffer buffer, UInt64 offset, MgIndexType indexType);
		void CmdBindVertexBuffers(UInt32 firstBinding, UInt32 bindingCount, MgBuffer[] pBuffers, UInt64[] pOffsets);
		void CmdDraw(UInt32 vertexCount, UInt32 instanceCount, UInt32 firstVertex, UInt32 firstInstance);
		void CmdDrawIndexed(UInt32 indexCount, UInt32 instanceCount, UInt32 firstIndex, Int32 vertexOffset, UInt32 firstInstance);
		void CmdDrawIndirect(MgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);
		void CmdDrawIndexedIndirect(MgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);
		void CmdDispatch(UInt32 x, UInt32 y, UInt32 z);
		void CmdDispatchIndirect(MgBuffer buffer, UInt64 offset);
		void CmdCopyBuffer(MgBuffer srcBuffer, MgBuffer dstBuffer, MgBufferCopy[] pRegions);
		void CmdCopyImage(MgImage srcImage, MgImageLayout srcImageLayout, MgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions);
		void CmdBlitImage(MgImage srcImage, MgImageLayout srcImageLayout, MgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter);
		void CmdCopyBufferToImage(MgBuffer srcBuffer, MgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions);
		void CmdCopyImageToBuffer(MgImage srcImage, MgImageLayout srcImageLayout, MgBuffer dstBuffer, MgBufferImageCopy[] pRegions);
		void CmdUpdateBuffer(MgBuffer dstBuffer, UInt64 dstOffset, UIntPtr dataSize, IntPtr pData);
		void CmdFillBuffer(MgBuffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data);
		void CmdClearColorImage(MgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges);
		void CmdClearDepthStencilImage(MgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges);
		void CmdClearAttachments(MgClearAttachment[] pAttachments, MgClearRect[] pRects);
		void CmdResolveImage(MgImage srcImage, MgImageLayout srcImageLayout, MgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions);
		void CmdSetEvent(MgEvent @event, MgPipelineStageFlagBits stageMask);
		void CmdResetEvent(MgEvent @event, MgPipelineStageFlagBits stageMask);
		void CmdWaitEvents(MgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers);
		void CmdPipelineBarrier(MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers);
		void CmdBeginQuery(MgQueryPool queryPool, UInt32 query, MgQueryControlFlagBits flags);
		void CmdEndQuery(MgQueryPool queryPool, UInt32 query);
		void CmdResetQueryPool(MgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount);
		void CmdWriteTimestamp(MgPipelineStageFlagBits pipelineStage, MgQueryPool queryPool, UInt32 query);
		void CmdCopyQueryPoolResults(MgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, MgBuffer dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags);
		void CmdPushConstants(MgPipelineLayout layout, MgShaderStageFlagBits stageFlags, UInt32 offset, UInt32 size, IntPtr pValues);
		void CmdBeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents);
		void CmdNextSubpass(MgSubpassContents contents);
		void CmdEndRenderPass();
		void CmdExecuteCommands(IMgCommandBuffer[] pCommandBuffers);
	}
}

