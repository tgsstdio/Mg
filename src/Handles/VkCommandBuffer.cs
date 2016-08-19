using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkCommandBuffer : IMgCommandBuffer
	{
		internal IntPtr Handle = IntPtr.Zero;
		internal VkCommandBuffer(IntPtr handle)
		{
			Handle = handle;
		}

		public Result BeginCommandBuffer(MgCommandBufferBeginInfo pBeginInfo)
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

		public void CmdBindPipeline(MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
		{
			throw new NotImplementedException();
		}

		public void CmdSetViewport(UInt32 firstViewport, MgViewport[] pViewports)
		{
			throw new NotImplementedException();
		}

		public void CmdSetScissor(UInt32 firstScissor, MgRect2D[] pScissors)
		{
			throw new NotImplementedException();
		}

		public void CmdSetLineWidth(Single lineWidth)
		{
			throw new NotImplementedException();
		}

		public void CmdSetDepthBias(Single depthBiasConstantFactor, Single depthBiasClamp, Single depthBiasSlopeFactor)
		{
			throw new NotImplementedException();
		}

		public void CmdSetBlendConstants(MgColor4f blendConstants)
		{
			throw new NotImplementedException();
		}

		public void CmdSetDepthBounds(Single minDepthBounds, Single maxDepthBounds)
		{
			throw new NotImplementedException();
		}

		public void CmdSetStencilCompareMask(MgStencilFaceFlagBits faceMask, UInt32 compareMask)
		{
			throw new NotImplementedException();
		}

		public void CmdSetStencilWriteMask(MgStencilFaceFlagBits faceMask, UInt32 writeMask)
		{
			throw new NotImplementedException();
		}

		public void CmdSetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference)
		{
			throw new NotImplementedException();
		}

		public void CmdBindDescriptorSets(MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, UInt32 firstSet, UInt32 descriptorSetCount, IMgDescriptorSet[] pDescriptorSets, UInt32[] pDynamicOffsets)
		{
			throw new NotImplementedException();
		}

		public void CmdBindIndexBuffer(IMgBuffer buffer, UInt64 offset, MgIndexType indexType)
		{
			throw new NotImplementedException();
		}

		public void CmdBindVertexBuffers(UInt32 firstBinding, IMgBuffer[] pBuffers, UInt64[] pOffsets)
		{
			throw new NotImplementedException();
		}

		public void CmdDraw(UInt32 vertexCount, UInt32 instanceCount, UInt32 firstVertex, UInt32 firstInstance)
		{
			throw new NotImplementedException();
		}

		public void CmdDrawIndexed(UInt32 indexCount, UInt32 instanceCount, UInt32 firstIndex, Int32 vertexOffset, UInt32 firstInstance)
		{
			throw new NotImplementedException();
		}

		public void CmdDrawIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride)
		{
			throw new NotImplementedException();
		}

		public void CmdDrawIndexedIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride)
		{
			throw new NotImplementedException();
		}

		public void CmdDispatch(UInt32 x, UInt32 y, UInt32 z)
		{
			throw new NotImplementedException();
		}

		public void CmdDispatchIndirect(IMgBuffer buffer, UInt64 offset)
		{
			throw new NotImplementedException();
		}

		public void CmdCopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
		{
			throw new NotImplementedException();
		}

		public void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
		{
			throw new NotImplementedException();
		}

		public void CmdBlitImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter)
		{
			throw new NotImplementedException();
		}

		public void CmdCopyBufferToImage(IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
		{
			throw new NotImplementedException();
		}

		public void CmdCopyImageToBuffer(IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
		{
			throw new NotImplementedException();
		}

		public void CmdUpdateBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UIntPtr dataSize, IntPtr pData)
		{
			throw new NotImplementedException();
		}

		public void CmdFillBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data)
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

		public void CmdClearAttachments(MgClearAttachment[] pAttachments, MgClearRect[] pRects)
		{
			throw new NotImplementedException();
		}

		public void CmdResolveImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions)
		{
			throw new NotImplementedException();
		}

		public void CmdSetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			throw new NotImplementedException();
		}

		public void CmdResetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			throw new NotImplementedException();
		}

		public void CmdWaitEvents(IMgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			throw new NotImplementedException();
		}

		public void CmdPipelineBarrier(MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			throw new NotImplementedException();
		}

		public void CmdBeginQuery(IMgQueryPool queryPool, UInt32 query, MgQueryControlFlagBits flags)
		{
			throw new NotImplementedException();
		}

		public void CmdEndQuery(IMgQueryPool queryPool, UInt32 query)
		{
			throw new NotImplementedException();
		}

		public void CmdResetQueryPool(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount)
		{
			throw new NotImplementedException();
		}

		public void CmdWriteTimestamp(MgPipelineStageFlagBits pipelineStage, IMgQueryPool queryPool, UInt32 query)
		{
			throw new NotImplementedException();
		}

		public void CmdCopyQueryPoolResults(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags)
		{
			throw new NotImplementedException();
		}

		public void CmdPushConstants(IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, UInt32 offset, UInt32 size, IntPtr pValues)
		{
			throw new NotImplementedException();
		}

		public void CmdBeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
		{
			throw new NotImplementedException();
		}

		public void CmdNextSubpass(MgSubpassContents contents)
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

	}
}
