using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLCommandBuffer : IMgCommandBuffer
	{
		private bool mIsRecording = false;
		private bool mIsExecutable = false;
		private bool mCanBeManuallyReset;

		private GLCmdComputeCommand mIncompleteComputeCommand;
		private GLCmdRenderPassCommand mIncompleteRenderPass;
		private List<GLCmdRenderPassCommand> mRenderPasses = new List<GLCmdRenderPassCommand>();
		private List<GLCmdDrawCommand> mIncompleteDraws = new List<GLCmdDrawCommand>();

		private IGLCmdBufferRepository mRepository;
		private IGLCmdVBOEntrypoint mVBO;
		private IGLImageFormatEntrypoint mImageFormat;
		public GLCommandBuffer (bool canBeManuallyReset, IGLCmdBufferRepository repository, IGLCmdVBOEntrypoint vbo, IGLImageFormatEntrypoint imgFormat)
		{
			mIsRecording = false;
			mIsExecutable = false;
			mCanBeManuallyReset = canBeManuallyReset;
			mRepository = repository;
			mVBO = vbo;
			mImageFormat = imgFormat;
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

		public CmdImageInstructionSet ImageInstructions { get; private set; }
		public CmdBufferInstructionSet InstructionSet { get; private set; }
		public Result EndCommandBuffer ()
		{
			mIsRecording = false;
			mIsExecutable = true;

			var mSetComposer = new CmdBufferInstructionSetTransformer (mVBO, mRepository);
			InstructionSet = mSetComposer.Compose (mRepository, mRenderPasses);

			ImageInstructions = new CmdImageInstructionSet ();
			ImageInstructions.LoadImageData = mImageCopies.ToArray ();				

			return Result.SUCCESS;
		}

		public void ResetAllData ()
		{
			mIncompleteRenderPass = null;
			mIncompleteComputeCommand = null;
			mRepository.Clear ();
			mRenderPasses.Clear ();
			mIncompleteDraws.Clear ();

			if (InstructionSet != null)
			{
				foreach (var vbo in InstructionSet.VBOs)
				{
					vbo.Dispose ();
				}
				InstructionSet = null;
			}
			ImageInstructions = null;

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
				var glPipeline = pipeline as IGLGraphicsPipeline;
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
			var parameter = new GLCmdDescriptorSetParameter ();		
			parameter.Bindpoint = pipelineBindPoint;
			parameter.Layout = layout;
			parameter.FirstSet = firstSet;
			parameter.DynamicOffsets = pDynamicOffsets;
			parameter.DescriptorSets = pDescriptorSets;
			mRepository.DescriptorSets.Add (parameter);
		}

		public void CmdBindIndexBuffer (IMgBuffer buffer, ulong offset, MgIndexType indexType)
		{
			var param = new GLCmdIndexBufferParameter ();
			param.buffer = buffer;
			param.offset = offset;
			param.indexType = indexType;
			mRepository.IndexBuffers.Add (param);
		}

		public void CmdBindVertexBuffers (uint firstBinding, IMgBuffer[] pBuffers, ulong[] pOffsets)
		{
			var param = new GLCmdVertexBufferParameter ();
			param.firstBinding = firstBinding;
			param.pBuffers = pBuffers;
			param.pOffsets = pOffsets;
			mRepository.VertexBuffers.Add (param);
		}

		void StoreDrawCommand (GLCmdDrawCommand command)
		{
			if (mRepository.MapRepositoryFields (ref command))
			{
				if (mIncompleteRenderPass != null)
				{
					mIncompleteRenderPass.DrawCommands.Add (command);
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

			var command = new GLCmdDrawCommand ();
			command.Draw = new GLCmdInternalDraw ();
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

			var command = new GLCmdDrawCommand ();
			command.DrawIndexed = new GLCmdInternalDrawIndexed ();
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

			var command = new GLCmdDrawCommand ();
			command.DrawIndirect = new GLCmdInternalDrawIndirect ();
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
			var command = new GLCmdDrawCommand ();
			command.DrawIndexedIndirect = new GLCmdInternalDrawIndexedIndirect();
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

		private List<GLCmdTexImageData> mImageCopies = new List<GLCmdTexImageData> ();
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
				var image = imgBarrier.Image as GLImage;
				if (image != null && imgBarrier.OldLayout == MgImageLayout.PREINITIALIZED && imgBarrier.NewLayout == MgImageLayout.SHADER_READ_ONLY_OPTIMAL)
				{
					var internalFormat = mImageFormat.GetGLFormat (image.Format, true);
//					PixelInternalFormat glInternalFormat;
//					PixelFormat glFormat;
//					PixelType glType;
//					image.Format.GetGLFormat (true, out glInternalFormat, out glFormat, out glType);

					var subResourceRange = imgBarrier.SubresourceRange;
					int layerEnd = (int)(subResourceRange.BaseArrayLayer + subResourceRange.LayerCount);
					int levelEnd = (int)(subResourceRange.BaseMipLevel + subResourceRange.LevelCount);
					for (int i = (int)subResourceRange.BaseArrayLayer; i < layerEnd; ++i)
					{
						var arrayDetail = image.ArrayLayers [i];
						for (int j = (int)subResourceRange.BaseMipLevel; j < levelEnd; ++j)
						{
							var levelDetail = arrayDetail.Levels [j];
							var copyCmd = new GLCmdTexImageData ();
							copyCmd.Target = image.ImageType;
							copyCmd.Level = j;
							copyCmd.Slice = i;
							copyCmd.Width = levelDetail.Width;
							copyCmd.Height = levelDetail.Height;
							copyCmd.Depth = levelDetail.Depth;
							copyCmd.Format = image.Format;
							copyCmd.PixelFormat = internalFormat.GLFormat;
							copyCmd.InternalFormat = internalFormat.InternalFormat;
							copyCmd.PixelType = internalFormat.GLType;
							copyCmd.TextureId = image.OriginalTextureId;
							if (levelDetail.SubresourceLayout.Size > (ulong)int.MaxValue)
							{
								throw new InvalidOperationException (string.Format ("array[{0}].Levels[{1}].SubresourceLayout.Size > int.MaxValue", i, j));
							}
							copyCmd.Size = (int)levelDetail.SubresourceLayout.Size;
							if (levelDetail.SubresourceLayout.Offset > (ulong)int.MaxValue)
							{
								throw new InvalidOperationException (string.Format ("array[{0}].Levels[{1}].SubresourceLayout.Offset > int.MaxValue", i, j));
							}
							copyCmd.Data = IntPtr.Add (image.Handle, (int)levelDetail.SubresourceLayout.Offset);
							mImageCopies.Add (copyCmd);
						}
					}
				}
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
			mIncompleteRenderPass = new GLCmdRenderPassCommand ();
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

