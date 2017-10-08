﻿using System;
using System.Diagnostics;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdCommandBuffer : IGLCommandBuffer
    {
		private bool mIsRecording = false;
		private bool mIsExecutable = false;
		private readonly bool mManuallyResettable;

		private readonly GLCmdCommandEncoder mCommandEncoder;
        public GLCmdCommandBufferRecord Record { get; private set; }

        public GLCmdCommandBuffer(bool canBeManuallyReset, GLCmdCommandEncoder encoder)
		{
			mIsRecording = false;
			mIsExecutable = false;
			mManuallyResettable = canBeManuallyReset;
			mCommandEncoder = encoder;
		}

		#region IMgCommandBuffer implementation

		public MgCommandBufferUsageFlagBits SubmissionRule { get; private set; }
		public bool IsQueueReady { get; set; }
		public Result BeginCommandBuffer(MgCommandBufferBeginInfo pBeginInfo)
		{
			SubmissionRule = pBeginInfo.Flags;
			IsQueueReady = true;

			mIsRecording = true;

			if ((pBeginInfo.Flags & MgCommandBufferUsageFlagBits.RENDER_PASS_CONTINUE_BIT)
					!= MgCommandBufferUsageFlagBits.RENDER_PASS_CONTINUE_BIT)
			{
				ResetAllData();
			}

			return Result.SUCCESS;
		}

		public Result EndCommandBuffer()
		{
			mIsRecording = false;
			mIsExecutable = true;

			// TODO : add encoder spacing for different encoder instructions                      
			Record = mCommandEncoder.AsRecord();

			return Result.SUCCESS;
		}

		public void ResetAllData()
		{
            // TODO : Clear item bags unless CONTINUE has been passed in
            if (Record != null)
            {
                Debug.Assert(Record.GraphicsGrid != null);
                Record.GraphicsGrid.Dispose();
            }                       
            mCommandEncoder.Clear();

			Record = mCommandEncoder.AsRecord();
		}

		public Result ResetCommandBuffer(MgCommandBufferResetFlagBits flags)
		{
			if (mManuallyResettable)
			{
				ResetAllData();
				// OTHERWISE WAIT FOR BULK RESET VIA COMMAND POOL
			}
			return Result.SUCCESS;
		}

		public void CmdBindPipeline(MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
		{
			if (pipeline == null)
				throw new ArgumentNullException(nameof(pipeline));

			if (pipelineBindPoint == MgPipelineBindPoint.COMPUTE)
			{
				mCommandEncoder.Compute.BindPipeline(pipeline);
			}
			else
			{
				mCommandEncoder.Graphics.BindPipeline(pipeline);
			}
		}

		public void CmdSetViewport(uint firstViewport, MgViewport[] pViewports)
		{
			mCommandEncoder.Graphics.SetViewports(firstViewport, pViewports);
		}

		public void CmdSetScissor(uint firstScissor, MgRect2D[] pScissors)
		{
			mCommandEncoder.Graphics.SetScissor(firstScissor, pScissors);
		}

		public void CmdSetLineWidth(float lineWidth)
		{
            mCommandEncoder.Graphics.SetLineWidth(lineWidth);
        }

		public void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			mCommandEncoder.Graphics.SetDepthBias(
				depthBiasConstantFactor,
				depthBiasClamp,
				depthBiasSlopeFactor);
		}

		public void CmdSetBlendConstants(MgColor4f blendConstants)
		{
			mCommandEncoder.Graphics.SetBlendConstants(blendConstants);
		}

		public void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds)
		{
            mCommandEncoder.Graphics.SetDepthBounds(minDepthBounds, maxDepthBounds);
		}

		public void CmdSetStencilCompareMask(MgStencilFaceFlagBits faceMask, uint compareMask)
		{
			mCommandEncoder.Graphics.SetStencilCompareMask(faceMask, compareMask);
		}

		public void CmdSetStencilWriteMask(MgStencilFaceFlagBits faceMask, uint writeMask)
		{
			mCommandEncoder.Graphics.SetStencilWriteMask(faceMask, writeMask);
		}

		public void CmdSetStencilReference(MgStencilFaceFlagBits faceMask, uint reference)
		{
			mCommandEncoder.Graphics.SetStencilReference(faceMask, reference);
		}

		public void CmdBindDescriptorSets(
			MgPipelineBindPoint pipelineBindPoint,
			IMgPipelineLayout layout,
			uint firstSet,
			IMgDescriptorSet[] pDescriptorSets,
			uint[] pDynamicOffsets)
		{
			if (pipelineBindPoint == MgPipelineBindPoint.GRAPHICS)
			{
                uint descriptorSetCount = pDescriptorSets != null ? (uint) pDescriptorSets.Length : 0U;
                mCommandEncoder.Graphics.BindDescriptorSets(layout,
															firstSet,
															descriptorSetCount,
															pDescriptorSets,
															pDynamicOffsets);
			}
		}

		public void CmdBindIndexBuffer(IMgBuffer buffer, ulong offset, MgIndexType indexType)
		{
			mCommandEncoder.Graphics.BindIndexBuffer(buffer, offset, indexType);
		}

		public void CmdBindVertexBuffers(uint firstBinding, IMgBuffer[] pBuffers, ulong[] pOffsets)
		{
			mCommandEncoder.Graphics.BindVertexBuffers(firstBinding, pBuffers, pOffsets);
		}

		public void CmdDraw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
		{
			mCommandEncoder.Graphics.Draw(vertexCount, instanceCount, firstVertex, firstInstance);
		}

		public void CmdDrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
		{
			mCommandEncoder.Graphics.DrawIndexed(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
		}

		public void CmdDrawIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            mCommandEncoder.Graphics.DrawIndirect(buffer, offset, drawCount, stride);
		}

		public void CmdDrawIndexedIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            mCommandEncoder.Graphics.DrawIndexedIndirect(buffer, offset, drawCount, stride);
		}

		public void CmdDispatch(uint x, uint y, uint z)
		{
			mCommandEncoder.Compute.Dispatch(x, y, z);
		}

		public void CmdDispatchIndirect(IMgBuffer buffer, ulong offset)
		{
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            mCommandEncoder.Compute.DispatchIndirect(buffer, offset);
		}

		public void CmdCopyBuffer (IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
		{
			mCommandEncoder.Blit.CopyBuffer(srcBuffer, dstBuffer, pRegions);
		}

		public void CmdCopyImage (IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
		{
			mCommandEncoder.Blit.CmdCopyImage(srcImage, srcImageLayout, dstImage, dstImageLayout, pRegions);
		}

		public void CmdBlitImage (IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyBufferToImage (IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
		{
			mCommandEncoder.Blit.CopyBufferToImage(srcBuffer, dstImage, dstImageLayout, pRegions);
		}

		public void CmdCopyImageToBuffer (IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
		{
			mCommandEncoder.Blit.CopyImageToBuffer(srcImage, srcImageLayout, dstBuffer, pRegions);
		}

		public void CmdUpdateBuffer (IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 dataSize, IntPtr pData)
		{
			throw new NotImplementedException ();
		}

		public void CmdFillBuffer (IMgBuffer dstBuffer, ulong dstOffset, ulong size, uint data)
		{
			throw new NotImplementedException ();
		}

		public void CmdClearColorImage (IMgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges)
		{
			throw new NotImplementedException ();
		}

		public void CmdClearDepthStencilImage (IMgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges)
		{
			throw new NotImplementedException ();
		}

		public void CmdClearAttachments (MgClearAttachment[] pAttachments, MgClearRect[] pRects)
		{
			throw new NotImplementedException ();
		}

		public void CmdResolveImage (IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdSetEvent (IMgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			throw new NotImplementedException ();
		}

		public void CmdResetEvent (IMgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			throw new NotImplementedException ();
		}

		public void CmdWaitEvents (IMgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			throw new NotImplementedException ();
		}

		public void CmdPipelineBarrier (MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{

		}

		public void CmdBeginQuery (IMgQueryPool queryPool, uint query, MgQueryControlFlagBits flags)
		{
			throw new NotImplementedException ();
		}

		public void CmdEndQuery (IMgQueryPool queryPool, uint query)
		{
			throw new NotImplementedException ();
		}

		public void CmdResetQueryPool (IMgQueryPool queryPool, uint firstQuery, uint queryCount)
		{
			throw new NotImplementedException ();
		}

		public void CmdWriteTimestamp (MgPipelineStageFlagBits pipelineStage, IMgQueryPool queryPool, uint query)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyQueryPoolResults (IMgQueryPool queryPool, uint firstQuery, uint queryCount, IMgBuffer dstBuffer, ulong dstOffset, ulong stride, MgQueryResultFlagBits flags)
		{
			throw new NotImplementedException ();
		}

		public void CmdPushConstants (IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, uint offset, uint size, IntPtr pValues)
		{
			throw new NotImplementedException ();
		}

		public void CmdBeginRenderPass (MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
		{
			mCommandEncoder.Graphics.BeginRenderPass(pRenderPassBegin, contents);
		}

		public void CmdNextSubpass (MgSubpassContents contents)
		{
			mCommandEncoder.Graphics.NextSubpass(contents);
		}

		public void CmdEndRenderPass ()
		{
			mCommandEncoder.Graphics.EndRenderPass ();
		}

		public void CmdExecuteCommands (IMgCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

