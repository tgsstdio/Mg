using System;
using System.Collections.Generic;
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtCommandBuffer2 : IMgCommandBuffer
	{
		private bool mIsRecording = false;
		private bool mIsExecutable = false;
		private bool mCanBeManuallyReset;

		private AmtCmdComputeCommand mIncompleteComputeCommand;
		private AmtCmdRenderPassCommand mIncompleteRenderPass;
		private List<AmtCmdRenderPassCommand> mRenderPasses = new List<AmtCmdRenderPassCommand>();
		private List<AmtDrawCommandEncoderState> mIncompleteDraws = new List<AmtDrawCommandEncoderState>();

		IAmtComputeEncoder mCompute;

		IAmtGraphicsEncoder mGraphics;

		public AmtCommandBuffer2(bool canBeManuallyReset, IAmtGraphicsEncoder graphics, IAmtComputeEncoder compute)
		{
			mIsRecording = false;
			mIsExecutable = false;
			mCanBeManuallyReset = canBeManuallyReset;
			mGraphics = graphics;
			mCompute = compute;
		}

		#region IMgCommandBuffer implementation

		public MgCommandBufferUsageFlagBits SubmissionRule { get; private set; }
		public bool IsQueueReady { get; set; }
		public Result BeginCommandBuffer(MgCommandBufferBeginInfo pBeginInfo)
		{
			SubmissionRule = pBeginInfo.Flags;
			IsQueueReady = true;

			mIsRecording = true;

			return Result.SUCCESS;
		}

		public Result EndCommandBuffer()
		{
			mIsRecording = false;
			mIsExecutable = true;

			// TODO : Convert loose item bag into "hard" item grids


			return Result.SUCCESS;
		}

		public void ResetAllData()
		{
			mIncompleteRenderPass = null;
			mIncompleteComputeCommand = null;
			// TODO : Clear item bags unless CONTINUE has been passed in
			mGraphics.Clear();
			mCompute.Clear();
			mRenderPasses.Clear();
			mIncompleteDraws.Clear();


			mImageCopies.Clear();
		}

		public Result ResetCommandBuffer(MgCommandBufferResetFlagBits flags)
		{
			if (mCanBeManuallyReset)
			{
				ResetAllData();
				// OTHERWISE WAIT FOR BULK RESET VIA COMMAND POOL
			}
			return Result.SUCCESS;
		}

		public void CmdBindPipeline(MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
		{
			if (pipelineBindPoint == MgPipelineBindPoint.COMPUTE)
			{
				mCompute.BindPipeline(pipeline);
			}
			else
			{
				mGraphics.BindPipeline(pipeline);
			}
		}

		public void CmdSetViewport(uint firstViewport, MgViewport[] pViewports)
		{
			mGraphics.SetViewports(firstViewport, pViewports);
		}

		public void CmdSetScissor(uint firstScissor, MgRect2D[] pScissors)
		{
			mGraphics.SetScissor(firstScissor, pScissors);
		}

		public void CmdSetLineWidth(float lineWidth)
		{
			// METAL : non supported
			// mGraphics.PushLineWidth(lineWidth);
		}

		public void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			mGraphics.SetDepthBias(
				depthBiasConstantFactor,
				depthBiasClamp,
				depthBiasSlopeFactor);
		}

		public void CmdSetBlendConstants(MgColor4f blendConstants)
		{
			mGraphics.SetBlendConstants(blendConstants);
		}

		public void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds)
		{
			// METAL : not supported
			//mRepository.PushDepthBounds(
			//	minDepthBounds,
			//	maxDepthBounds
			//);
		}

		public void CmdSetStencilCompareMask(MgStencilFaceFlagBits faceMask, uint compareMask)
		{
			mGraphics.SetStencilCompareMask(faceMask, compareMask);
		}

		public void CmdSetStencilWriteMask(MgStencilFaceFlagBits faceMask, uint writeMask)
		{
			mGraphics.SetStencilWriteMask(faceMask, writeMask);
		}

		public void CmdSetStencilReference(MgStencilFaceFlagBits faceMask, uint reference)
		{
			mGraphics.SetStencilReference(faceMask, reference);
		}

		public void CmdBindDescriptorSets(
			MgPipelineBindPoint pipelineBindPoint,
			IMgPipelineLayout layout,
			uint firstSet,
			uint descriptorSetCount,
			IMgDescriptorSet[] pDescriptorSets,
			uint[] pDynamicOffsets)
		{
			//var parameter = new AmtDescriptorSetRecordingState();
			//parameter.Bindpoint = pipelineBindPoint;
			//parameter.Layout = layout;
			//parameter.FirstSet = firstSet;
			//parameter.DynamicOffsets = pDynamicOffsets;
			//parameter.DescriptorSets = pDescriptorSets;
			//mRepository.DescriptorSets.Add(parameter);
		}

		public void CmdBindIndexBuffer(IMgBuffer buffer, ulong offset, MgIndexType indexType)
		{
			mGraphics.BindIndexBuffer(buffer, offset, indexType);
		}

		public void CmdBindVertexBuffers(uint firstBinding, IMgBuffer[] pBuffers, ulong[] pOffsets)
		{
			//var param = new AmtVertexBufferEncoderState();
			//param.firstBinding = firstBinding;
			//param.pBuffers = pBuffers;
			//param.pOffsets = pOffsets;
			//mRepository.VertexBuffers.Add(param);
		}

		public void CmdDraw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
		{
			mGraphics.Draw(vertexCount, instanceCount, firstVertex, firstInstance);
		}

		public void CmdDrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
		{
			mGraphics.DrawIndexed(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
		}

		public void CmdDrawIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
			mGraphics.DrawIndirect(buffer, offset, drawCount, stride);
		}

		public void CmdDrawIndexedIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
			mGraphics.DrawIndexedIndirect(buffer, offset, drawCount, stride);
		}

		public void CmdDispatch(uint x, uint y, uint z)
		{
			mCompute.Dispatch(x, y, z);
		}

		public void CmdDispatchIndirect(IMgBuffer buffer, ulong offset)
		{
			mCompute.DispatchIndirect(buffer, offset);
		}

		public void CmdCopyBuffer (IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyImage (IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdBlitImage (IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyBufferToImage (IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyImageToBuffer (IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
		{
			throw new NotImplementedException ();
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

		private List<AmtCmdTexImageData> mImageCopies = new List<AmtCmdTexImageData> ();
		public void CmdPipelineBarrier (MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			if (pImageMemoryBarriers != null)
			{
				foreach (var imgBarrier in pImageMemoryBarriers)
				{	
					LoadImageData (imgBarrier);
				}
			}

		}

		/**
		 * Image memory barriers initialisation are co-opted to call glCompressedTexSubImage1/2/3DExt
		 * to load data into the texture storage
	     **/
		void LoadImageData (MgImageMemoryBarrier imgBarrier)
		{
			if (imgBarrier != null)
			{

			}
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
			mGraphics.BeginRenderPass(pRenderPassBegin, contents);
		}

		public void CmdNextSubpass (MgSubpassContents contents)
		{
			mGraphics.NextSubpass(contents);
		}

		public void CmdEndRenderPass ()
		{
			mGraphics.EndRenderPass ();
		}

		public void CmdExecuteCommands (IMgCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

