using System;
using System.Collections.Generic;
using System.Diagnostics;

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

		private IAmtCmdBufferRepository mRepository;
		public AmtCommandBuffer2 (bool canBeManuallyReset, IAmtCmdBufferRepository repository)
		{
			mIsRecording = false;
			mIsExecutable = false;
			mCanBeManuallyReset = canBeManuallyReset;
			mRepository = repository;

		}

		#region IMgCommandBuffer implementation

		public MgCommandBufferUsageFlagBits SubmissionRule { get; private set; }
		public bool IsQueueReady { get; set; }
		public Result BeginCommandBuffer (MgCommandBufferBeginInfo pBeginInfo)
		{
			SubmissionRule = pBeginInfo.Flags;
			IsQueueReady = true;

			mIsRecording = true;

			return Result.SUCCESS;
		}

		public Result EndCommandBuffer ()
		{
			mIsRecording = false;
			mIsExecutable = true;

			// Convert loose item bag into "hard" item grids


			return Result.SUCCESS;
		}

		public void ResetAllData ()
		{
			mIncompleteRenderPass = null;
			mIncompleteComputeCommand = null;
			// TODO : Clear item bags unless CONTINUE has been passed in
			mRepository.Clear ();
			mRenderPasses.Clear ();
			mIncompleteDraws.Clear ();


			mImageCopies.Clear ();
		}

		public Result ResetCommandBuffer (MgCommandBufferResetFlagBits flags)
		{
			if (mCanBeManuallyReset)
			{				
				ResetAllData ();
				// OTHERWISE WAIT FOR BULK RESET VIA COMMAND POOL
			}
			return Result.SUCCESS;
		}

		public void CmdBindPipeline (MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
		{
			if (pipelineBindPoint == MgPipelineBindPoint.COMPUTE)
			{
				mIncompleteComputeCommand.Pipeline = pipeline;
			}
			else
			{
				var glPipeline = (AmtGraphicsPipeline) pipeline;
				mRepository.PushGraphicsPipeline(glPipeline);
			}
		}

		public void CmdSetViewport (uint firstViewport, MgViewport[] pViewports)
		{
			mRepository.PushViewports (firstViewport, pViewports);
		}

		public void CmdSetScissor (uint firstScissor, MgRect2D[] pScissors)
		{
			mRepository.PushScissors (firstScissor, pScissors);
		}

		public void CmdSetLineWidth (float lineWidth)
		{
			mRepository.PushLineWidth (lineWidth);
		}

		public void CmdSetDepthBias (float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			mRepository.PushDepthBias (
				depthBiasConstantFactor,
				depthBiasClamp,
				depthBiasSlopeFactor);
		}

		public void CmdSetBlendConstants (MgColor4f blendConstants)
		{
			mRepository.PushBlendConstants (blendConstants);
		}

		public void CmdSetDepthBounds (float minDepthBounds, float maxDepthBounds)
		{
			mRepository.PushDepthBounds(
				minDepthBounds,
				maxDepthBounds
			);
		}

		public void CmdSetStencilCompareMask (MgStencilFaceFlagBits faceMask, uint compareMask)
		{
			mRepository.SetCompareMask (faceMask, compareMask);
		}

		public void CmdSetStencilWriteMask (MgStencilFaceFlagBits faceMask, uint writeMask)
		{
			mRepository.SetWriteMask (faceMask, writeMask);
		}

		public void CmdSetStencilReference (MgStencilFaceFlagBits faceMask, uint reference)
		{
			mRepository.SetStencilReference (faceMask, reference);
		}

		public void CmdBindDescriptorSets (
			MgPipelineBindPoint pipelineBindPoint,
			IMgPipelineLayout layout,
			uint firstSet,
			uint descriptorSetCount,
			IMgDescriptorSet[] pDescriptorSets,
			uint[] pDynamicOffsets)
		{
			var parameter = new AmtDescriptorSetRecordingState ();		
			parameter.Bindpoint = pipelineBindPoint;
			parameter.Layout = layout;
			parameter.FirstSet = firstSet;
			parameter.DynamicOffsets = pDynamicOffsets;
			parameter.DescriptorSets = pDescriptorSets;
			mRepository.DescriptorSets.Add (parameter);
		}

		public void CmdBindIndexBuffer (IMgBuffer buffer, ulong offset, MgIndexType indexType)
		{
			var param = new AmtCmdIndexBufferParameter ();
			param.buffer = buffer;
			param.offset = offset;
			param.indexType = indexType;
			mRepository.IndexBuffers.Add (param);
		}

		public void CmdBindVertexBuffers (uint firstBinding, IMgBuffer[] pBuffers, ulong[] pOffsets)
		{
			var param = new AmtVertexBufferEncoderState ();
			param.firstBinding = firstBinding;
			param.pBuffers = pBuffers;
			param.pOffsets = pOffsets;
			mRepository.VertexBuffers.Add (param);
		}

		void StoreDrawCommand (AmtDrawCommandEncoderState command)
		{
			if (mRepository.MapRepositoryFields (ref command))
			{
				if (mIncompleteRenderPass != null)
				{
					// TODO : add draw command to instruction list
					//mIncompleteRenderPass.DrawCommands.Add (command);
				} 
				else
				{
					mIncompleteDraws.Add (command);
				}
			}
		}

		public void CmdDraw (uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
		{
			//void glDrawArraysInstancedBaseInstance(GLenum mode​, GLint first​, GLsizei count​, GLsizei primcount​, GLuint baseinstance​);
			//mDrawCommands.Add (mIncompleteDrawCommand);
			// first => firstVertex
			// count => vertexCount
			// primcount => instanceCount Specifies the number of instances of the indexed geometry that should be drawn.
			// baseinstance => firstInstance Specifies the base instance for use in fetching instanced vertex attributes.

			var command = new AmtDrawCommandEncoderState ();
			command.Draw.vertexCount = vertexCount;
			command.Draw.instanceCount = instanceCount;
			command.Draw.firstVertex = firstVertex;
			command.Draw.firstInstance = firstInstance;

			StoreDrawCommand(command);
		}

		public void CmdDrawIndexed (uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
		{
			// void glDrawElementsInstancedBaseVertex(GLenum mode​, GLsizei count​, GLenum type​, GLvoid *indices​, GLsizei primcount​, GLint basevertex​);
			// count => indexCount Specifies the number of elements to be rendered. (divide by elements)
			// indices => firstIndex Specifies a byte offset (cast to a pointer type) (multiple by data size)
			// primcount => instanceCount Specifies the number of instances of the indexed geometry that should be drawn.
			// basevertex => vertexOffset Specifies a constant that should be added to each element of indices​ when chosing elements from the enabled vertex arrays.
				// TODO : need to handle negetive offset
			//mDrawCommands.Add (mIncompleteDrawCommand);

			var command = new AmtDrawCommandEncoderState ();
			command.DrawIndexed = new AmtDrawIndexedEncoderState ();
			command.DrawIndexed.indexCount = indexCount;
			command.DrawIndexed.instanceCount = instanceCount;
			command.DrawIndexed.firstIndex = firstIndex;
			command.DrawIndexed.vertexOffset = vertexOffset;
			command.DrawIndexed.firstInstance = firstInstance;

			StoreDrawCommand(command);
		}

		public void CmdDrawIndirect (IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
			// ARB_multi_draw_indirect
//			typedef struct VkDrawIndirectCommand {
//				uint32_t    vertexCount;
//				uint32_t    instanceCount; 
//				uint32_t    firstVertex; 
//				uint32_t    firstInstance;
//			} VkDrawIndirectCommand;
			// glMultiDrawArraysIndirect 
			//void glMultiDrawArraysIndirect(GLenum mode​, const void *indirect​, GLsizei drawcount​, GLsizei stride​);
			// indirect => buffer + offset IntPtr
			// drawCount => drawCount
			// stride => stride
//			typedef  struct {
//				uint  count;
//				uint  instanceCount;
//				uint  first;
//				uint  baseInstance;
//			} DrawArraysIndirectCommand;
			//mDrawCommands.Add (mIncompleteDrawCommand);

			var command = new AmtDrawCommandEncoderState ();
			command.DrawIndirect = new AmtDrawIndirectEncoderState ();
			command.DrawIndirect.buffer = buffer;
			command.DrawIndirect.offset = offset;
			command.DrawIndirect.drawCount = drawCount;
			command.DrawIndirect.stride = stride;

			StoreDrawCommand(command);
		}

		public void CmdDrawIndexedIndirect (IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
//			typedef struct VkDrawIndexedIndirectCommand {
//				uint32_t    indexCount;
//				uint32_t    instanceCount;
//				uint32_t    firstIndex;
//				int32_t     vertexOffset;
//				uint32_t    firstInstance;
//			} VkDrawIndexedIndirectCommand;
			// void glMultiDrawElementsIndirect(GLenum mode​, GLenum type​, const void *indirect​, GLsizei drawcount​, GLsizei stride​);
			// indirect  => buffer + offset (IntPtr)
			// drawcount => drawcount
			// stride => stride
//			glDrawElementsInstancedBaseVertexBaseInstance(mode,
//				cmd->count,
//				type,
//				cmd->firstIndex * size-of-type,
//				cmd->instanceCount,
//				cmd->baseVertex,
//				cmd->baseInstance);
//			typedef  struct {
//				uint  count;
//				uint  instanceCount;
//				uint  firstIndex;
//				uint  baseVertex; // TODO: negetive index
//				uint  baseInstance;
//			} DrawElementsIndirectCommand;
			//mDrawCommands.Add (mIncompleteDrawCommand);
			var command = new AmtDrawCommandEncoderState ();
			command.DrawIndexedIndirect = new AmtDrawIndexedIndirectEncoderState();
			command.DrawIndexedIndirect.buffer = buffer;
			command.DrawIndexedIndirect.offset = offset;
			command.DrawIndexedIndirect.drawCount = drawCount;
			command.DrawIndexedIndirect.stride = stride;

			StoreDrawCommand(command);
		}

		public void CmdDispatch (uint x, uint y, uint z)
		{
			throw new NotImplementedException ();
		}

		public void CmdDispatchIndirect (IMgBuffer buffer, ulong offset)
		{
			throw new NotImplementedException ();
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
			if (pRenderPassBegin != null)
				throw new ArgumentNullException(nameof(pRenderPassBegin));

			var bRenderPass = (AmtRenderPass)pRenderPassBegin.RenderPass;
			Debug.Assert(bRenderPass != null, nameof(pRenderPassBegin.RenderPass) + " is null");

			var bFramebuffer = (AmtFramebuffer)pRenderPassBegin.Framebuffer;
			Debug.Assert(bFramebuffer != null, nameof(pRenderPassBegin.Framebuffer) + " is null");

			var clearValuesCount = pRenderPassBegin.ClearValues != null ? pRenderPassBegin.ClearValues.Length : 0;

			// TRANSLATE CLEAR VALUES TO DOUBLE VEC4 OR STENCIL, DEPTH

			mIncompleteRenderPass = new AmtCmdRenderPassCommand ();
			mIncompleteRenderPass.Origin = pRenderPassBegin.RenderPass;
			mIncompleteRenderPass.ClearValues = pRenderPassBegin.ClearValues;
			mIncompleteRenderPass.Contents = contents;
		}

		public void CmdNextSubpass (MgSubpassContents contents)
		{
			throw new NotImplementedException ();
		}

		public void CmdEndRenderPass ()
		{
			mRenderPasses.Add (mIncompleteRenderPass);
		}

		public void CmdExecuteCommands (IMgCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

