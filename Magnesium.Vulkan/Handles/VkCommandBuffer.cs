using System;
using Magnesium.Vulkan.Functions.CommandBuffer;

namespace Magnesium.Vulkan
{
    public class VkCommandBuffer : IMgCommandBuffer
	{
		internal VkCommandBufferInfo Info { get; }
		internal VkCommandBuffer(IntPtr handle)
		{
            Info = new VkCommandBufferInfo(handle);
		}

		public MgResult BeginCommandBuffer(MgCommandBufferBeginInfo pBeginInfo)
        {
            return VkBeginCommandBufferSection.BeginCommandBuffer(Info, pBeginInfo);
        }

        public MgResult EndCommandBuffer()
        {
            return VkEndCommandBufferSection.EndCommandBuffer(Info);
        }

        public MgResult ResetCommandBuffer(MgCommandBufferResetFlagBits flags)
        {
            return VkResetCommandBufferSection.ResetCommandBuffer(Info, flags);
        }

        public void CmdBindPipeline(MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
        {
            VkCmdBindPipelineSection.CmdBindPipeline(Info, pipelineBindPoint, pipeline);
        }

        public void CmdSetViewport(UInt32 firstViewport, MgViewport[] pViewports)
        {
            VkCmdSetViewportSection.CmdSetViewport(Info, firstViewport, pViewports);
        }

        public void CmdSetScissor(UInt32 firstScissor, MgRect2D[] pScissors)
        {
            VkCmdSetScissorSection.CmdSetScissor(Info, firstScissor, pScissors);
        }

        public void CmdSetLineWidth(float lineWidth)
        {
            VkCmdSetLineWidthSection.CmdSetLineWidth(Info, lineWidth);
        }

        public void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
        {
            VkCmdSetDepthBiasSection.CmdSetDepthBias(Info, depthBiasConstantFactor, depthBiasClamp, depthBiasSlopeFactor);
        }

        public void CmdSetBlendConstants(MgColor4f blendConstants)
        {
            VkCmdSetBlendConstantsSection.CmdSetBlendConstants(Info, blendConstants);
        }

        public void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds)
        {
            VkCmdSetDepthBoundsSection.CmdSetDepthBounds(Info, minDepthBounds, maxDepthBounds);
        }

        public void CmdSetStencilCompareMask(MgStencilFaceFlagBits faceMask, UInt32 compareMask)
        {
            VkCmdSetStencilCompareMaskSection.CmdSetStencilCompareMask(Info, faceMask, compareMask);
        }

        public void CmdSetStencilWriteMask(MgStencilFaceFlagBits faceMask, UInt32 writeMask)
        {
            VkCmdSetStencilWriteMaskSection.CmdSetStencilWriteMask(Info, faceMask, writeMask);
        }

        public void CmdSetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference)
        {
            VkCmdSetStencilReferenceSection.CmdSetStencilReference(Info, faceMask, reference);
        }

        public void CmdBindDescriptorSets(MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, UInt32 firstSet, UInt32 descriptorSetCount, IMgDescriptorSet[] pDescriptorSets, UInt32[] pDynamicOffsets)
        {
            VkCmdBindDescriptorSetsSection.CmdBindDescriptorSets(Info, pipelineBindPoint, layout, firstSet, descriptorSetCount, pDescriptorSets, pDynamicOffsets);
        }

        public void CmdBindIndexBuffer(IMgBuffer buffer, UInt64 offset, MgIndexType indexType)
        {
            VkCmdBindIndexBufferSection.CmdBindIndexBuffer(Info, buffer, offset, indexType);
        }

        public void CmdBindVertexBuffers(UInt32 firstBinding, IMgBuffer[] pBuffers, UInt64[] pOffsets)
        {
            VkCmdBindVertexBuffersSection.CmdBindVertexBuffers(Info, firstBinding, pBuffers, pOffsets);
        }

        public void CmdDraw(UInt32 vertexCount, UInt32 instanceCount, UInt32 firstVertex, UInt32 firstInstance)
        {
            VkCmdDrawSection.CmdDraw(Info, vertexCount, instanceCount, firstVertex, firstInstance);
        }

        public void CmdDrawIndexed(UInt32 indexCount, UInt32 instanceCount, UInt32 firstIndex, Int32 vertexOffset, UInt32 firstInstance)
        {
            VkCmdDrawIndexedSection.CmdDrawIndexed(Info, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
        }

        public void CmdDrawIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride)
        {
            VkCmdDrawIndirectSection.CmdDrawIndirect(Info, buffer, offset, drawCount, stride);
        }

        public void CmdDrawIndexedIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride)
        {
            VkCmdDrawIndexedIndirectSection.CmdDrawIndexedIndirect(Info, buffer, offset, drawCount, stride);
        }

        public void CmdDispatch(UInt32 x, UInt32 y, UInt32 z)
        {
            VkCmdDispatchSection.CmdDispatch(Info, x, y, z);
        }

        public void CmdDispatchIndirect(IMgBuffer buffer, UInt64 offset)
        {
            VkCmdDispatchIndirectSection.CmdDispatchIndirect(Info, buffer, offset);
        }

        public void CmdCopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
        {
            VkCmdCopyBufferSection.CmdCopyBuffer(Info, srcBuffer, dstBuffer, pRegions);
        }

        public void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
        {
            VkCmdCopyImageSection.CmdCopyImage(Info, srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions);
        }

        public void CmdBlitImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter)
        {
            VkCmdBlitImageSection.CmdBlitImage(Info, srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions, filter);
        }

        public void CmdCopyBufferToImage(IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
        {
            VkCmdCopyBufferToImageSection.CmdCopyBufferToImage(Info, srcBuffer, dstImage, dstImageLayout, pRegions);
        }

        public void CmdCopyImageToBuffer(IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
        {
            VkCmdCopyImageToBufferSection.CmdCopyImageToBuffer(Info, srcImage, srcImageLayout, dstBuffer, pRegions);
        }

        public void CmdUpdateBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 dataSize, IntPtr pData)
        {
            VkCmdUpdateBufferSection.CmdUpdateBuffer(Info, dstBuffer, dstOffset, dataSize, pData);
        }

        public void CmdFillBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data)
        {
            VkCmdFillBufferSection.CmdFillBuffer(Info, dstBuffer, dstOffset, size, data);
        }

        public void CmdClearColorImage(IMgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges)
        {
            VkCmdClearColorImageSection.CmdClearColorImage(Info, image, imageLayout, pColor, pRanges);
        }

        public void CmdClearDepthStencilImage(IMgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges)
        {
            VkCmdClearDepthStencilImageSection.CmdClearDepthStencilImage(Info, image, imageLayout, pDepthStencil, pRanges);
        }

        public void CmdClearAttachments(MgClearAttachment[] pAttachments, MgClearRect[] pRects)
        {
            VkCmdClearAttachmentsSection.CmdClearAttachments(Info, pAttachments, pRects);
        }

        public void CmdResolveImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions)
        {
            VkCmdResolveImageSection.CmdResolveImage(Info, srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions);
        }

        public void CmdSetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask)
        {
            VkCmdSetEventSection.CmdSetEvent(Info, @event, stageMask);
        }

        public void CmdResetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask)
        {
            VkCmdResetEventSection.CmdResetEvent(Info, @event, stageMask);
        }

        public void CmdWaitEvents(IMgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
        {
            VkCmdWaitEventsSection.CmdWaitEvents(Info, pEvents, srcStageMask, dstStageMask, pMemoryBarriers, pBufferMemoryBarriers, pImageMemoryBarriers);
        }

        public void CmdPipelineBarrier(MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
        {
            VkCmdPipelineBarrierSection.CmdPipelineBarrier(Info, srcStageMask, dstStageMask, dependencyFlags, pMemoryBarriers, pBufferMemoryBarriers, pImageMemoryBarriers);
        }

        public void CmdBeginQuery(IMgQueryPool queryPool, UInt32 query, MgQueryControlFlagBits flags)
        {
            VkCmdBeginQuerySection.CmdBeginQuery(Info, queryPool, query, flags);
        }

        public void CmdEndQuery(IMgQueryPool queryPool, UInt32 query)
        {
            VkCmdEndQuerySection.CmdEndQuery(Info, queryPool, query);
        }

        public void CmdResetQueryPool(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount)
        {
            VkCmdResetQueryPoolSection.CmdResetQueryPool(Info, queryPool, firstQuery, queryCount);
        }

        public void CmdWriteTimestamp(MgPipelineStageFlagBits pipelineStage, IMgQueryPool queryPool, UInt32 query)
        {
            VkCmdWriteTimestampSection.CmdWriteTimestamp(Info, pipelineStage, queryPool, query);
        }

        public void CmdCopyQueryPoolResults(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags)
        {
            VkCmdCopyQueryPoolResultsSection.CmdCopyQueryPoolResults(Info, queryPool, firstQuery, queryCount, dstBuffer, dstOffset, stride, flags);
        }

        public void CmdPushConstants(IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, UInt32 offset, UInt32 size, IntPtr pValues)
        {
            VkCmdPushConstantsSection.CmdPushConstants(Info, layout, stageFlags, offset, size, pValues);
        }

        public void CmdBeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
        {
            VkCmdBeginRenderPassSection.CmdBeginRenderPass(Info, pRenderPassBegin, contents);
        }

        public void CmdNextSubpass(MgSubpassContents contents)
        {
            VkCmdNextSubpassSection.CmdNextSubpass(Info, contents);
        }

        public void CmdEndRenderPass()
        {
            VkCmdEndRenderPassSection.CmdEndRenderPass(Info);
        }

        public void CmdExecuteCommands(IMgCommandBuffer[] pCommandBuffers)
        {
            VkCmdExecuteCommandsSection.CmdExecuteCommands(Info, pCommandBuffers);
        }

        public void CmdBeginConditionalRenderingEXT(MgConditionalRenderingBeginInfoEXT pConditionalRenderingBegin)
        {
            VkCmdBeginConditionalRenderingEXTSection.CmdBeginConditionalRenderingEXT(Info, pConditionalRenderingBegin);
        }

        public void CmdProcessCommandsNVX(MgCmdProcessCommandsInfoNVX pProcessCommandsInfo)
        {
            VkCmdProcessCommandsNVXSection.CmdProcessCommandsNVX(Info, pProcessCommandsInfo);
        }

        public void CmdSetDiscardRectangleEXT(uint firstDiscardRectangle, MgRect2D[] discardRectangles)
        {
            VkCmdSetDiscardRectangleEXTSection.CmdSetDiscardRectangleEXT(Info, firstDiscardRectangle, discardRectangles);
        }

        public void CmdSetExclusiveScissorNV(uint firstExclusiveScissor, MgRect2D[] exclusiveScissors)
        {
            VkCmdSetExclusiveScissorNVSection.CmdSetExclusiveScissorNV(Info, firstExclusiveScissor, exclusiveScissors);
        }
    }
}
