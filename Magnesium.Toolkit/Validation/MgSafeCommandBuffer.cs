using System;
namespace Magnesium.Toolkit
{
    public class MgSafeCommandBuffer : IMgCommandBuffer
	{
		internal IMgCommandBuffer mImpl = null;
		internal MgSafeCommandBuffer(IMgCommandBuffer impl)
		{
			mImpl = impl;
		}

		public MgResult BeginCommandBuffer(MgCommandBufferBeginInfo pBeginInfo) {
			Validation.CommandBuffer.BeginCommandBuffer.Validate(pBeginInfo);
			return mImpl.BeginCommandBuffer(pBeginInfo);
		}

		public MgResult EndCommandBuffer() {
			return mImpl.EndCommandBuffer();
		}

		public MgResult ResetCommandBuffer(MgCommandBufferResetFlagBits flags) {
			Validation.CommandBuffer.ResetCommandBuffer.Validate(flags);
			return mImpl.ResetCommandBuffer(flags);
		}

		public void CmdBindPipeline(MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline) {
			Validation.CommandBuffer.CmdBindPipeline.Validate(pipelineBindPoint, pipeline);
			mImpl.CmdBindPipeline(pipelineBindPoint, pipeline);
		}

		public void CmdSetViewport(UInt32 firstViewport, MgViewport[] pViewports) {
			Validation.CommandBuffer.CmdSetViewport.Validate(firstViewport, pViewports);
			mImpl.CmdSetViewport(firstViewport, pViewports);
		}

		public void CmdSetScissor(UInt32 firstScissor, MgRect2D[] pScissors) {
			Validation.CommandBuffer.CmdSetScissor.Validate(firstScissor, pScissors);
			mImpl.CmdSetScissor(firstScissor, pScissors);
		}

		public void CmdSetLineWidth(float lineWidth) {
			Validation.CommandBuffer.CmdSetLineWidth.Validate(lineWidth);
			mImpl.CmdSetLineWidth(lineWidth);
		}

		public void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor) {
			Validation.CommandBuffer.CmdSetDepthBias.Validate(depthBiasConstantFactor, depthBiasClamp, depthBiasSlopeFactor);
			mImpl.CmdSetDepthBias(depthBiasConstantFactor, depthBiasClamp, depthBiasSlopeFactor);
		}

		public void CmdSetBlendConstants(MgColor4f blendConstants) {
			Validation.CommandBuffer.CmdSetBlendConstants.Validate(blendConstants);
			mImpl.CmdSetBlendConstants(blendConstants);
		}

		public void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds) {
			Validation.CommandBuffer.CmdSetDepthBounds.Validate(minDepthBounds, maxDepthBounds);
			mImpl.CmdSetDepthBounds(minDepthBounds, maxDepthBounds);
		}

		public void CmdSetStencilCompareMask(MgStencilFaceFlagBits faceMask, UInt32 compareMask) {
			Validation.CommandBuffer.CmdSetStencilCompareMask.Validate(faceMask, compareMask);
			mImpl.CmdSetStencilCompareMask(faceMask, compareMask);
		}

		public void CmdSetStencilWriteMask(MgStencilFaceFlagBits faceMask, UInt32 writeMask) {
			Validation.CommandBuffer.CmdSetStencilWriteMask.Validate(faceMask, writeMask);
			mImpl.CmdSetStencilWriteMask(faceMask, writeMask);
		}

		public void CmdSetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference) {
			Validation.CommandBuffer.CmdSetStencilReference.Validate(faceMask, reference);
			mImpl.CmdSetStencilReference(faceMask, reference);
		}

		public void CmdBindDescriptorSets(MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, UInt32 firstSet, IMgDescriptorSet[] pDescriptorSets, UInt32[] pDynamicOffsets) {
			Validation.CommandBuffer.CmdBindDescriptorSets.Validate(pipelineBindPoint, layout, firstSet, pDescriptorSets, pDynamicOffsets);
			mImpl.CmdBindDescriptorSets(pipelineBindPoint, layout, firstSet, pDescriptorSets, pDynamicOffsets);
		}

		public void CmdBindIndexBuffer(IMgBuffer buffer, UInt64 offset, MgIndexType indexType) {
			Validation.CommandBuffer.CmdBindIndexBuffer.Validate(buffer, offset, indexType);
			mImpl.CmdBindIndexBuffer(buffer, offset, indexType);
		}

		public void CmdBindVertexBuffers(UInt32 firstBinding, IMgBuffer[] pBuffers, UInt64[] pOffsets) {
			Validation.CommandBuffer.CmdBindVertexBuffers.Validate(firstBinding, pBuffers, pOffsets);
			mImpl.CmdBindVertexBuffers(firstBinding, pBuffers, pOffsets);
		}

		public void CmdDraw(UInt32 vertexCount, UInt32 instanceCount, UInt32 firstVertex, UInt32 firstInstance) {
			Validation.CommandBuffer.CmdDraw.Validate(vertexCount, instanceCount, firstVertex, firstInstance);
			mImpl.CmdDraw(vertexCount, instanceCount, firstVertex, firstInstance);
		}

		public void CmdDrawIndexed(UInt32 indexCount, UInt32 instanceCount, UInt32 firstIndex, Int32 vertexOffset, UInt32 firstInstance) {
			Validation.CommandBuffer.CmdDrawIndexed.Validate(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
			mImpl.CmdDrawIndexed(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
		}

		public void CmdDrawIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride) {
			Validation.CommandBuffer.CmdDrawIndirect.Validate(buffer, offset, drawCount, stride);
			mImpl.CmdDrawIndirect(buffer, offset, drawCount, stride);
		}

		public void CmdDrawIndexedIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride) {
			Validation.CommandBuffer.CmdDrawIndexedIndirect.Validate(buffer, offset, drawCount, stride);
			mImpl.CmdDrawIndexedIndirect(buffer, offset, drawCount, stride);
		}

		public void CmdDispatch(UInt32 x, UInt32 y, UInt32 z) {
			Validation.CommandBuffer.CmdDispatch.Validate(x, y, z);
			mImpl.CmdDispatch(x, y, z);
		}

		public void CmdDispatchIndirect(IMgBuffer buffer, UInt64 offset) {
			Validation.CommandBuffer.CmdDispatchIndirect.Validate(buffer, offset);
			mImpl.CmdDispatchIndirect(buffer, offset);
		}

		public void CmdCopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions) {
			Validation.CommandBuffer.CmdCopyBuffer.Validate(srcBuffer, dstBuffer, pRegions);
			mImpl.CmdCopyBuffer(srcBuffer, dstBuffer, pRegions);
		}

		public void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions) {
			Validation.CommandBuffer.CmdCopyImage.Validate(srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions);
			mImpl.CmdCopyImage(srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions);
		}

		public void CmdBlitImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter) {
			Validation.CommandBuffer.CmdBlitImage.Validate(srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions, filter);
			mImpl.CmdBlitImage(srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions, filter);
		}

		public void CmdCopyBufferToImage(IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions) {
			Validation.CommandBuffer.CmdCopyBufferToImage.Validate(srcBuffer, dstImage, dstImageLayout, pRegions);
			mImpl.CmdCopyBufferToImage(srcBuffer, dstImage, dstImageLayout, pRegions);
		}

		public void CmdCopyImageToBuffer(IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions) {
			Validation.CommandBuffer.CmdCopyImageToBuffer.Validate(srcImage, srcImageLayout, dstBuffer, pRegions);
			mImpl.CmdCopyImageToBuffer(srcImage, srcImageLayout, dstBuffer, pRegions);
		}

		public void CmdUpdateBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 dataSize, IntPtr pData) {
			Validation.CommandBuffer.CmdUpdateBuffer.Validate(dstBuffer, dstOffset, dataSize, pData);
			mImpl.CmdUpdateBuffer(dstBuffer, dstOffset, dataSize, pData);
		}

		public void CmdFillBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data) {
			Validation.CommandBuffer.CmdFillBuffer.Validate(dstBuffer, dstOffset, size, data);
			mImpl.CmdFillBuffer(dstBuffer, dstOffset, size, data);
		}

		public void CmdClearColorImage(IMgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges) {
			Validation.CommandBuffer.CmdClearColorImage.Validate(image, imageLayout, pColor, pRanges);
			mImpl.CmdClearColorImage(image, imageLayout, pColor, pRanges);
		}

		public void CmdClearDepthStencilImage(IMgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges) {
			Validation.CommandBuffer.CmdClearDepthStencilImage.Validate(image, imageLayout, pDepthStencil, pRanges);
			mImpl.CmdClearDepthStencilImage(image, imageLayout, pDepthStencil, pRanges);
		}

		public void CmdClearAttachments(MgClearAttachment[] pAttachments, MgClearRect[] pRects) {
			Validation.CommandBuffer.CmdClearAttachments.Validate(pAttachments, pRects);
			mImpl.CmdClearAttachments(pAttachments, pRects);
		}

		public void CmdResolveImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions) {
			Validation.CommandBuffer.CmdResolveImage.Validate(srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions);
			mImpl.CmdResolveImage(srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions);
		}

		public void CmdSetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask) {
			Validation.CommandBuffer.CmdSetEvent.Validate(@event, stageMask);
			mImpl.CmdSetEvent(@event, stageMask);
		}

		public void CmdResetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask) {
			Validation.CommandBuffer.CmdResetEvent.Validate(@event, stageMask);
			mImpl.CmdResetEvent(@event, stageMask);
		}

		public void CmdWaitEvents(IMgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers) {
			Validation.CommandBuffer.CmdWaitEvents.Validate(pEvents, srcStageMask, dstStageMask, pMemoryBarriers, pBufferMemoryBarriers, pImageMemoryBarriers);
			mImpl.CmdWaitEvents(pEvents, srcStageMask, dstStageMask, pMemoryBarriers, pBufferMemoryBarriers, pImageMemoryBarriers);
		}

		public void CmdPipelineBarrier(MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers) {
			Validation.CommandBuffer.CmdPipelineBarrier.Validate(srcStageMask, dstStageMask, dependencyFlags, pMemoryBarriers, pBufferMemoryBarriers, pImageMemoryBarriers);
			mImpl.CmdPipelineBarrier(srcStageMask, dstStageMask, dependencyFlags, pMemoryBarriers, pBufferMemoryBarriers, pImageMemoryBarriers);
		}

		public void CmdBeginQuery(IMgQueryPool queryPool, UInt32 query, MgQueryControlFlagBits flags) {
			Validation.CommandBuffer.CmdBeginQuery.Validate(queryPool, query, flags);
			mImpl.CmdBeginQuery(queryPool, query, flags);
		}

		public void CmdEndQuery(IMgQueryPool queryPool, UInt32 query) {
			Validation.CommandBuffer.CmdEndQuery.Validate(queryPool, query);
			mImpl.CmdEndQuery(queryPool, query);
		}

		public void CmdResetQueryPool(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount) {
			Validation.CommandBuffer.CmdResetQueryPool.Validate(queryPool, firstQuery, queryCount);
			mImpl.CmdResetQueryPool(queryPool, firstQuery, queryCount);
		}

		public void CmdWriteTimestamp(MgPipelineStageFlagBits pipelineStage, IMgQueryPool queryPool, UInt32 query) {
			Validation.CommandBuffer.CmdWriteTimestamp.Validate(pipelineStage, queryPool, query);
			mImpl.CmdWriteTimestamp(pipelineStage, queryPool, query);
		}

		public void CmdCopyQueryPoolResults(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags) {
			Validation.CommandBuffer.CmdCopyQueryPoolResults.Validate(queryPool, firstQuery, queryCount, dstBuffer, dstOffset, stride, flags);
			mImpl.CmdCopyQueryPoolResults(queryPool, firstQuery, queryCount, dstBuffer, dstOffset, stride, flags);
		}

		public void CmdPushConstants(IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, UInt32 offset, UInt32 size, IntPtr pValues) {
			Validation.CommandBuffer.CmdPushConstants.Validate(layout, stageFlags, offset, size, pValues);
			mImpl.CmdPushConstants(layout, stageFlags, offset, size, pValues);
		}

		public void CmdBeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents) {
			Validation.CommandBuffer.CmdBeginRenderPass.Validate(pRenderPassBegin, contents);
			mImpl.CmdBeginRenderPass(pRenderPassBegin, contents);
		}

		public void CmdNextSubpass(MgSubpassContents contents) {
			Validation.CommandBuffer.CmdNextSubpass.Validate(contents);
			mImpl.CmdNextSubpass(contents);
		}

		public void CmdEndRenderPass() {
			mImpl.CmdEndRenderPass();
		}

		public void CmdExecuteCommands(IMgCommandBuffer[] pCommandBuffers) {
			Validation.CommandBuffer.CmdExecuteCommands.Validate(pCommandBuffers);
			mImpl.CmdExecuteCommands(pCommandBuffers);
		}

		public void CmdBeginConditionalRenderingEXT(MgConditionalRenderingBeginInfoEXT pConditionalRenderingBegin) {
			Validation.CommandBuffer.CmdBeginConditionalRenderingEXT.Validate(pConditionalRenderingBegin);
			mImpl.CmdBeginConditionalRenderingEXT(pConditionalRenderingBegin);
		}

		public void CmdProcessCommandsNVX(MgCmdProcessCommandsInfoNVX pProcessCommandsInfo) {
			Validation.CommandBuffer.CmdProcessCommandsNVX.Validate(pProcessCommandsInfo);
			mImpl.CmdProcessCommandsNVX(pProcessCommandsInfo);
		}

		public void CmdSetDiscardRectangleEXT(UInt32 firstDiscardRectangle, MgRect2D[] discardRectangles) {
			Validation.CommandBuffer.CmdSetDiscardRectangleEXT.Validate(firstDiscardRectangle, discardRectangles);
			mImpl.CmdSetDiscardRectangleEXT(firstDiscardRectangle, discardRectangles);
		}

		public void CmdSetExclusiveScissorNV(UInt32 firstExclusiveScissor, MgRect2D[] exclusiveScissors) {
			Validation.CommandBuffer.CmdSetExclusiveScissorNV.Validate(firstExclusiveScissor, exclusiveScissors);
			mImpl.CmdSetExclusiveScissorNV(firstExclusiveScissor, exclusiveScissors);
		}

	}
}
