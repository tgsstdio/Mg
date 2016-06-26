using System;

namespace Magnesium
{
    // CommandBuffer
    public interface IMgCommandBuffer
	{
		Result BeginCommandBuffer(MgCommandBufferBeginInfo pBeginInfo);
		Result EndCommandBuffer();
		Result ResetCommandBuffer(MgCommandBufferResetFlagBits flags);
		void CmdBindPipeline(MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline);
		void CmdSetViewport(UInt32 firstViewport, MgViewport[] pViewports);
		void CmdSetScissor(UInt32 firstScissor, MgRect2D[] pScissors);
		void CmdSetLineWidth(float lineWidth);
		void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);
		void CmdSetBlendConstants(MgColor4f blendConstants); // 4
		void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds);
		void CmdSetStencilCompareMask(MgStencilFaceFlagBits faceMask, UInt32 compareMask);
		void CmdSetStencilWriteMask(MgStencilFaceFlagBits faceMask, UInt32 writeMask);
		void CmdSetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference);
		void CmdBindDescriptorSets(MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, UInt32 firstSet, UInt32 descriptorSetCount, MgDescriptorSet[] pDescriptorSets, UInt32[] pDynamicOffsets);
		void CmdBindIndexBuffer(IMgBuffer buffer, UInt64 offset, MgIndexType indexType);
		void CmdBindVertexBuffers(UInt32 firstBinding, UInt32 bindingCount, IMgBuffer[] pBuffers, UInt64[] pOffsets);
		void CmdDraw(UInt32 vertexCount, UInt32 instanceCount, UInt32 firstVertex, UInt32 firstInstance);
		void CmdDrawIndexed(UInt32 indexCount, UInt32 instanceCount, UInt32 firstIndex, Int32 vertexOffset, UInt32 firstInstance);
		void CmdDrawIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);
		void CmdDrawIndexedIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);
		void CmdDispatch(UInt32 x, UInt32 y, UInt32 z);
		void CmdDispatchIndirect(IMgBuffer buffer, UInt64 offset);
		void CmdCopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions);
		void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions);
		void CmdBlitImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter);
		void CmdCopyBufferToImage(IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions);
		void CmdCopyImageToBuffer(IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions);
		void CmdUpdateBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UIntPtr dataSize, IntPtr pData);
		void CmdFillBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data);
		void CmdClearColorImage(IMgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges);
		void CmdClearDepthStencilImage(IMgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges);
		void CmdClearAttachments(MgClearAttachment[] pAttachments, MgClearRect[] pRects);
		void CmdResolveImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions);
		void CmdSetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask);
		void CmdResetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask);
		void CmdWaitEvents(IMgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers);
		void CmdPipelineBarrier(MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers);
		void CmdBeginQuery(IMgQueryPool queryPool, UInt32 query, MgQueryControlFlagBits flags);
		void CmdEndQuery(IMgQueryPool queryPool, UInt32 query);
		void CmdResetQueryPool(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount);
		void CmdWriteTimestamp(MgPipelineStageFlagBits pipelineStage, IMgQueryPool queryPool, UInt32 query);
		void CmdCopyQueryPoolResults(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags);
		void CmdPushConstants(IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, UInt32 offset, UInt32 size, IntPtr pValues);
		void CmdBeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents);
		void CmdNextSubpass(MgSubpassContents contents);
		void CmdEndRenderPass();
		void CmdExecuteCommands(IMgCommandBuffer[] pCommandBuffers);
	}
}

