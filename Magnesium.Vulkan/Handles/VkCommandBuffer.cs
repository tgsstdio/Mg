using Magnesium;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkCommandBuffer : IMgCommandBuffer
	{
		internal IntPtr Handle { get; private set; }
		internal VkCommandBuffer(IntPtr handle)
		{
			Handle = handle;
		}

		public Result BeginCommandBuffer(MgCommandBufferBeginInfo pBeginInfo)
		{
			IntPtr inheritanceInfo = IntPtr.Zero;
			try
			{
				var param_0 = new VkCommandBufferBeginInfo();
				param_0.sType = VkStructureType.StructureTypeCommandBufferBeginInfo;
				param_0.pNext = IntPtr.Zero;
				param_0.flags = (VkCommandBufferUsageFlags)pBeginInfo.Flags;

				if (pBeginInfo.InheritanceInfo != null)
				{
					var ihData = new VkCommandBufferInheritanceInfo();
					ihData.sType = VkStructureType.StructureTypeCommandBufferInheritanceInfo;
					ihData.pNext = IntPtr.Zero;

					{
						UInt64 internalPtr = 0UL;
						var container = pBeginInfo.InheritanceInfo.RenderPass;
						if (container != null)
						{
							var rp = (VkRenderPass) container;
							Debug.Assert(rp != null);
							internalPtr = rp.Handle;
						}
						ihData.renderPass = internalPtr;
					}

					ihData.subpass = pBeginInfo.InheritanceInfo.Subpass;

					{
						UInt64 internalPtr = 0UL;
						var container = pBeginInfo.InheritanceInfo.Framebuffer;
						if (container != null)
						{
							var fb = (VkFramebuffer) container;
							Debug.Assert(fb != null);
							internalPtr = fb.Handle;
						}
						ihData.framebuffer = internalPtr;
					}

					ihData.occlusionQueryEnable = new VkBool32 { Value = pBeginInfo.InheritanceInfo.OcclusionQueryEnable ? 1U : 0U };
					ihData.queryFlags = (VkQueryControlFlags)pBeginInfo.InheritanceInfo.QueryFlags;
					ihData.pipelineStatistics = (VkQueryPipelineStatisticFlags)pBeginInfo.InheritanceInfo.PipelineStatistics;

					// Copy data
					inheritanceInfo = Marshal.AllocHGlobal(Marshal.SizeOf(ihData));
					Marshal.StructureToPtr(ihData, inheritanceInfo, false);
				}

				param_0.pInheritanceInfo = inheritanceInfo;

				return Interops.vkBeginCommandBuffer(this.Handle, ref param_0);
			}
			finally
			{
				if (inheritanceInfo != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(inheritanceInfo);
				}
			}
		}

		public Result EndCommandBuffer()
		{
			return Interops.vkEndCommandBuffer(this.Handle);
		}

		public Result ResetCommandBuffer(MgCommandBufferResetFlagBits flags)
		{
			return Interops.vkResetCommandBuffer(this.Handle, (VkCommandBufferResetFlags)flags);
		}

		public void CmdBindPipeline(MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
		{
			var bPipeline = (VkPipeline) pipeline;
			Debug.Assert(bPipeline != null);

			Interops.vkCmdBindPipeline(this.Handle, (VkPipelineBindPoint)pipelineBindPoint, bPipeline.Handle);
		}

		public void CmdSetViewport(UInt32 firstViewport, MgViewport[] pViewports)
		{
			var viewportHandle = GCHandle.Alloc(pViewports, GCHandleType.Pinned);

			try
			{
				unsafe
				{
					var viewportCount = (UInt32)pViewports.Length;
					var pinnedObject = viewportHandle.AddrOfPinnedObject();

					var viewports = (MgViewport*)pinnedObject.ToPointer();

					Interops.vkCmdSetViewport(this.Handle, firstViewport, viewportCount, viewports);
				}
			}
			finally
			{
				viewportHandle.Free();
			}
		}

		public void CmdSetScissor(UInt32 firstScissor, MgRect2D[] pScissors)
		{
			var scissorHandle = GCHandle.Alloc(pScissors, GCHandleType.Pinned);

			try
			{
				unsafe
				{
					var count = (UInt32)pScissors.Length;
					var pinnedObject = scissorHandle.AddrOfPinnedObject();

					var scissors = (MgRect2D*)pinnedObject.ToPointer();

					Interops.vkCmdSetScissor(this.Handle, firstScissor, count, scissors);
				}
			}
			finally
			{
				scissorHandle.Free();
			}
		}

		public void CmdSetLineWidth(float lineWidth)
		{
			Interops.vkCmdSetLineWidth(this.Handle, lineWidth);
		}

		public void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			Interops.vkCmdSetDepthBias(this.Handle, depthBiasConstantFactor, depthBiasClamp, depthBiasSlopeFactor);
		}

		public void CmdSetBlendConstants(MgColor4f blendConstants)
		{
			var color = new float[4];
			color[0] = blendConstants.R;
			color[1] = blendConstants.G;
			color[2] = blendConstants.B;
			color[3] = blendConstants.A;

			// TODO : figure a way to directly pass in
			Interops.vkCmdSetBlendConstants(this.Handle, color);
		}

		public void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds)
		{
			Interops.vkCmdSetDepthBounds(this.Handle, minDepthBounds, maxDepthBounds);
		}

		public void CmdSetStencilCompareMask(MgStencilFaceFlagBits faceMask, UInt32 compareMask)
		{
			Interops.vkCmdSetStencilCompareMask(this.Handle, (VkStencilFaceFlags)faceMask, compareMask);
		}

		public void CmdSetStencilWriteMask(MgStencilFaceFlagBits faceMask, UInt32 writeMask)
		{
			Interops.vkCmdSetStencilWriteMask(this.Handle, (VkStencilFaceFlags)faceMask, writeMask);
		}

		public void CmdSetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference)
		{
			Interops.vkCmdSetStencilReference(this.Handle, (VkStencilFaceFlags)faceMask, reference);
		}

		public void CmdBindDescriptorSets(MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, UInt32 firstSet, IMgDescriptorSet[] pDescriptorSets, UInt32[] pDynamicOffsets)
		{

            var bLayout = (VkPipelineLayout)layout;
            Debug.Assert(bLayout != null);

            var stride = Marshal.SizeOf(typeof(IntPtr));
            var descriptorSetCount = pDescriptorSets != null ? (uint) pDescriptorSets.Length : 0U;
            IntPtr sets = Marshal.AllocHGlobal((int)(stride * descriptorSetCount));

            var src = new ulong[descriptorSetCount];
            for (uint i = 0; i < descriptorSetCount; ++i)
            {
                var bDescSet = (VkDescriptorSet)pDescriptorSets[i];
                Debug.Assert(bDescSet != null);
                src[i] = bDescSet.Handle;
            }

            // var dynamic (uint)pDynamicOffsets.Length
            uint dynamicOffsetCount = pDynamicOffsets != null ? (uint)pDynamicOffsets.Length : 0U;
            Interops.vkCmdBindDescriptorSets(this.Handle, (VkPipelineBindPoint)pipelineBindPoint, bLayout.Handle, firstSet, descriptorSetCount, src, dynamicOffsetCount, pDynamicOffsets);
		}

		public void CmdBindIndexBuffer(IMgBuffer buffer, UInt64 offset, MgIndexType indexType)
		{
			var bBuffer = (VkBuffer) buffer;
			Debug.Assert(bBuffer != null);

			Interops.vkCmdBindIndexBuffer(this.Handle, bBuffer.Handle, offset, (VkIndexType)indexType);
		}

		public void CmdBindVertexBuffers(UInt32 firstBinding, IMgBuffer[] pBuffers, UInt64[] pOffsets)
		{
			var bindingCount = (uint) pBuffers.Length;
			var src = new ulong[pBuffers.Length];
			for (uint i = 0; i < bindingCount; ++i)
			{
				var bBuffer = (VkBuffer) pBuffers[i];
				Debug.Assert(bBuffer != null);
				src[i] = bBuffer.Handle;
			}

			Interops.vkCmdBindVertexBuffers(this.Handle, firstBinding, bindingCount, src, pOffsets); 
		}

		public void CmdDraw(UInt32 vertexCount, UInt32 instanceCount, UInt32 firstVertex, UInt32 firstInstance)
		{
			Interops.vkCmdDraw(this.Handle, vertexCount, instanceCount, firstVertex, firstInstance);
		}

		public void CmdDrawIndexed(UInt32 indexCount, UInt32 instanceCount, UInt32 firstIndex, Int32 vertexOffset, UInt32 firstInstance)
		{
			Interops.vkCmdDrawIndexed(this.Handle, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
		}

		public void CmdDrawIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride)
		{
			var bBuffer = (VkBuffer) buffer;
			Debug.Assert(bBuffer != null);

			Interops.vkCmdDrawIndirect(this.Handle, bBuffer.Handle, offset, drawCount, stride);
		}

		public void CmdDrawIndexedIndirect(IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride)
		{
			var bBuffer = (VkBuffer) buffer;
			Debug.Assert(bBuffer != null);

			Interops.vkCmdDrawIndexedIndirect(this.Handle, bBuffer.Handle, offset, drawCount, stride);
		}

		public void CmdDispatch(UInt32 x, UInt32 y, UInt32 z)
		{
			Interops.vkCmdDispatch(this.Handle, x, y, z);
		}

		public void CmdDispatchIndirect(IMgBuffer buffer, UInt64 offset)
		{
			var bBuffer = (VkBuffer) buffer;
			Debug.Assert(bBuffer != null);

			Interops.vkCmdDispatchIndirect(this.Handle, bBuffer.Handle, offset);
		}

		public void CmdCopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
		{
			var bBuffer_src = (VkBuffer) srcBuffer;
			Debug.Assert(bBuffer_src != null);

			var bBuffer_dst = (VkBuffer) dstBuffer;
			Debug.Assert(bBuffer_dst != null);

			var handle = GCHandle.Alloc(pRegions, GCHandleType.Pinned);

			try
			{
				unsafe
				{
					var regionCount = (uint)pRegions.Length;
					var region = handle.AddrOfPinnedObject();

					MgBufferCopy* regions = (MgBufferCopy*)region.ToPointer();

					Interops.vkCmdCopyBuffer(this.Handle, bBuffer_src.Handle, bBuffer_dst.Handle, regionCount, regions);
				}
			}
			finally
			{
				handle.Free();
			}
		}

		public void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
		{
			var bSrcImage = (VkImage) srcImage;
			Debug.Assert(bSrcImage != null);

			var bDstImage = (VkImage) dstImage;
			Debug.Assert(bDstImage != null);


			var handle = GCHandle.Alloc(pRegions, GCHandleType.Pinned);

			try
			{
				unsafe
				{
					var regionCount = (uint)pRegions.Length;
					var region = handle.AddrOfPinnedObject();

					MgImageCopy* regions = (MgImageCopy*)region.ToPointer();
					Interops.vkCmdCopyImage(this.Handle, bSrcImage.Handle, (VkImageLayout)srcImageLayout, bDstImage.Handle, (VkImageLayout)dstImageLayout, regionCount, regions);
				}
			}
			finally
			{
				handle.Free();
			}
		}

		public void CmdBlitImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter)
		{
			var bSrcImage = (VkImage) srcImage;
			Debug.Assert(bSrcImage != null);
			var bDstImage = (VkImage) dstImage;
			Debug.Assert(bDstImage != null);

			Interops.vkCmdBlitImage(this.Handle, bSrcImage.Handle, srcImageLayout, bDstImage.Handle, dstImageLayout, (uint)pRegions.Length, pRegions, (VkFilter)filter);
		}

		public void CmdCopyBufferToImage(IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
		{
			var bSrcBuffer = (VkBuffer) srcBuffer;
			Debug.Assert(bSrcBuffer != null);
			var bDstImage = (VkImage) dstImage;
			Debug.Assert(bDstImage != null);

			var handle = GCHandle.Alloc(pRegions, GCHandleType.Pinned);

			try
			{
				unsafe
				{
					var regionCount = (uint)pRegions.Length;
					var region = handle.AddrOfPinnedObject();

					var regions = (MgBufferImageCopy*)region.ToPointer();

					Interops.vkCmdCopyBufferToImage(this.Handle, bSrcBuffer.Handle, bDstImage.Handle, dstImageLayout, regionCount, regions);
				}
			}
			finally
			{
				handle.Free();
			}
		}

		public void CmdCopyImageToBuffer(IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
		{
			var bSrcImage = (VkImage) srcImage;
			Debug.Assert(bSrcImage != null);
			var bDstBuffer = (VkBuffer) dstBuffer;
			Debug.Assert(bDstBuffer != null);

			var handle = GCHandle.Alloc(pRegions, GCHandleType.Pinned);

			try
			{
				unsafe
				{
					var regionCount = (uint)pRegions.Length;
					var regionAddress = handle.AddrOfPinnedObject();

					var pinnedArray = (MgBufferImageCopy*)regionAddress.ToPointer();

					Interops.vkCmdCopyImageToBuffer(this.Handle, bSrcImage.Handle, srcImageLayout, bDstBuffer.Handle, regionCount, pinnedArray); 
				}
			}
			finally
			{
				handle.Free();
			}
		}

		public void CmdUpdateBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 dataSize, IntPtr pData)
		{
			var bDstBuffer = (VkBuffer) dstBuffer;
			Debug.Assert(bDstBuffer != null);

			Interops.vkCmdUpdateBuffer(this.Handle, bDstBuffer.Handle, dstOffset, dataSize, pData);
		}

		public void CmdFillBuffer(IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data)
		{
			var bDstBuffer = (VkBuffer) dstBuffer;
			Debug.Assert(bDstBuffer != null);

			Interops.vkCmdFillBuffer(this.Handle, bDstBuffer.Handle, dstOffset, size, data);
		}

		public void CmdClearColorImage(IMgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges)
		{
			var bImage = (VkImage) image;
			Debug.Assert(bImage != null);

			Interops.vkCmdClearColorImage(this.Handle, bImage.Handle, imageLayout, pColor, (uint)pRanges.Length, pRanges); 
		}

		public void CmdClearDepthStencilImage(IMgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges)
		{
			var bImage = (VkImage) image;
			Debug.Assert(bImage != null);

			Interops.vkCmdClearDepthStencilImage(this.Handle, bImage.Handle, imageLayout, pDepthStencil, (uint)pRanges.Length, pRanges); 
		}

		public void CmdClearAttachments(MgClearAttachment[] pAttachments, MgClearRect[] pRects)
		{
			var attachmentHandle = GCHandle.Alloc(pAttachments, GCHandleType.Pinned);
			var rectsHandle = GCHandle.Alloc(pRects, GCHandleType.Pinned);

			try
			{
				unsafe
				{
					var attachmentCount = (uint)pAttachments.Length;
					var attachment = attachmentHandle.AddrOfPinnedObject();

					var rectCount = (uint)pRects.Length;
					var rects = rectsHandle.AddrOfPinnedObject();
					Interops.vkCmdClearAttachments(this.Handle, attachmentCount, (Magnesium.MgClearAttachment*)attachment.ToPointer(), rectCount, (Magnesium.MgClearRect*)rects.ToPointer());
			   }
			}
			finally
			{
				rectsHandle.Free();
				attachmentHandle.Free();
			}
		}

		public void CmdResolveImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions)
		{
			var bSrcImage = (VkImage) srcImage;
			Debug.Assert(bSrcImage != null);
			var bDstImage = (VkImage) dstImage;
			Debug.Assert(bDstImage != null);

			var handle = GCHandle.Alloc(pRegions, GCHandleType.Pinned);

			try
			{
				unsafe
				{
					var regionCount = (uint)pRegions.Length;
					var regionAddress = handle.AddrOfPinnedObject();

					var pinnedArray = (MgImageResolve*)regionAddress.ToPointer();

					Interops.vkCmdResolveImage(this.Handle, bSrcImage.Handle, srcImageLayout, bDstImage.Handle, dstImageLayout, regionCount, pinnedArray);
				}
			}
			finally
			{
				handle.Free();
			}
		}

		public void CmdSetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			var bEvent = (VkEvent) @event;
			Debug.Assert(bEvent != null);

			Interops.vkCmdSetEvent(this.Handle, bEvent.Handle, (VkPipelineStageFlags)stageMask);
		}

		public void CmdResetEvent(IMgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			var bEvent = (VkEvent) @event;
			Debug.Assert(bEvent != null);

			Interops.vkCmdResetEvent(this.Handle, bEvent.Handle, (VkPipelineStageFlags)stageMask);
		}

		public void CmdWaitEvents(IMgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			unsafe
			{
				var eventHandles = stackalloc UInt64[pEvents.Length];
				var eventCount = (uint)pEvents.Length;
				for (var i = 0; i < eventCount; ++i)
				{
					var bEvent = (VkEvent) pEvents[i];
					Debug.Assert(bEvent != null);
					eventHandles[i] = bEvent.Handle;
				}

				var memBarrierCount = 0U;
				VkMemoryBarrier* pMemBarriers = null;
				if (pMemoryBarriers != null)
				{
					memBarrierCount = (uint)pMemoryBarriers.Length;
					var tempMem = stackalloc VkMemoryBarrier[pMemoryBarriers.Length];
					for (var i = 0; i < memBarrierCount; ++i)
					{
						tempMem[i] = new VkMemoryBarrier
						{
							sType = VkStructureType.StructureTypeMemoryBarrier,
							pNext = IntPtr.Zero,
							srcAccessMask = (VkAccessFlags)pMemoryBarriers[i].SrcAccessMask,
							dstAccessMask = (VkAccessFlags)pMemoryBarriers[i].DstAccessMask,
						};
					}
					pMemBarriers = tempMem;
				}

				uint bufBarrierCount = 0;
				VkBufferMemoryBarrier* pBufBarriers = null;
				if (pBufferMemoryBarriers != null)
				{
					bufBarrierCount = (uint)pBufferMemoryBarriers.Length;
					var tempBuf = stackalloc VkBufferMemoryBarrier[pBufferMemoryBarriers.Length];
					for (var i = 0; i < bufBarrierCount; ++i)
					{
						var current = pBufferMemoryBarriers[i];
						var bBuffer = (VkBuffer) current.Buffer;
						Debug.Assert(bBuffer != null);

						tempBuf[i] = new VkBufferMemoryBarrier
						{
							sType = VkStructureType.StructureTypeBufferMemoryBarrier,
							pNext = IntPtr.Zero,
							dstAccessMask = (VkAccessFlags)current.DstAccessMask,
							srcAccessMask = (VkAccessFlags)current.SrcAccessMask,
							srcQueueFamilyIndex = current.SrcQueueFamilyIndex,
							dstQueueFamilyIndex = current.DstQueueFamilyIndex,
							buffer = bBuffer.Handle,
							offset = current.Offset,
							size = current.Size,
						};
					}
					pBufBarriers = tempBuf;
				}

				uint imgBarriersCount = 0;
				VkImageMemoryBarrier* pImgBarriers = null;

				if (pImageMemoryBarriers != null)
				{
					imgBarriersCount = (uint)pImageMemoryBarriers.Length;
					var tempImg = stackalloc VkImageMemoryBarrier[pImageMemoryBarriers.Length];
					for (var i = 0; i < bufBarrierCount; ++i)
					{
						var current = pImageMemoryBarriers[i];
						var bImage = (VkImage) current.Image;
						Debug.Assert(bImage != null);

						tempImg[i] = new VkImageMemoryBarrier
						{
							sType = VkStructureType.StructureTypeImageMemoryBarrier,
							pNext = IntPtr.Zero,
							dstAccessMask = (VkAccessFlags)current.DstAccessMask,
							srcAccessMask = (VkAccessFlags)current.SrcAccessMask,
							oldLayout = (Magnesium.Vulkan.VkImageLayout)current.OldLayout,
							newLayout = (Magnesium.Vulkan.VkImageLayout)current.NewLayout,
							srcQueueFamilyIndex = current.SrcQueueFamilyIndex,
							dstQueueFamilyIndex = current.DstQueueFamilyIndex,
							image = bImage.Handle,
							subresourceRange = new VkImageSubresourceRange
							{
								aspectMask = (Magnesium.Vulkan.VkImageAspectFlags)current.SubresourceRange.AspectMask,
								baseArrayLayer = current.SubresourceRange.BaseArrayLayer,
								baseMipLevel = current.SubresourceRange.BaseMipLevel,
								layerCount = current.SubresourceRange.LayerCount,
								levelCount = current.SubresourceRange.LevelCount,
							}
						};
					}
					pImgBarriers = tempImg;
				}

				Interops.vkCmdWaitEvents(
					this.Handle,
					eventCount,
					eventHandles, 
					(VkPipelineStageFlags)srcStageMask,
					(VkPipelineStageFlags)dstStageMask,
					memBarrierCount,
					pMemBarriers,
					bufBarrierCount,
					pBufBarriers,
					imgBarriersCount,
					pImgBarriers);
			}

		}

		public void CmdPipelineBarrier(MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			unsafe
			{
				var memBarrierCount = pMemoryBarriers != null ? (uint)pMemoryBarriers.Length : 0U;
                var pMemBarriers = stackalloc VkMemoryBarrier[(int)memBarrierCount];

				for (var i = 0; i < memBarrierCount; ++i)
				{
                    pMemBarriers[i] = new VkMemoryBarrier
					{
						sType = VkStructureType.StructureTypeMemoryBarrier,
						pNext = IntPtr.Zero,
						srcAccessMask = (VkAccessFlags)pMemoryBarriers[i].SrcAccessMask,
						dstAccessMask = (VkAccessFlags)pMemoryBarriers[i].DstAccessMask,
					};
				}


				uint bufBarrierCount = pBufferMemoryBarriers != null ? (uint)pBufferMemoryBarriers.Length : 0U;
				var pBufBarriers = stackalloc VkBufferMemoryBarrier[(int) bufBarrierCount];
				for (var j = 0; j < bufBarrierCount; ++j)
				{
					var current = pBufferMemoryBarriers[j];
					var bBuffer = (VkBuffer) current.Buffer;
					Debug.Assert(bBuffer != null);

                    pBufBarriers[j] = new VkBufferMemoryBarrier
					{
						sType = VkStructureType.StructureTypeBufferMemoryBarrier,
						pNext = IntPtr.Zero,
						dstAccessMask = (VkAccessFlags)current.DstAccessMask,
						srcAccessMask = (VkAccessFlags)current.SrcAccessMask,
						srcQueueFamilyIndex = current.SrcQueueFamilyIndex,
						dstQueueFamilyIndex = current.DstQueueFamilyIndex,
						buffer = bBuffer.Handle,
						offset = current.Offset,
						size = current.Size,
					};
				}


                uint imgBarriersCount = pImageMemoryBarriers != null ? (uint)pImageMemoryBarriers.Length : 0U;
                var pImgBarriers = stackalloc VkImageMemoryBarrier[(int) imgBarriersCount];

				for (var k = 0; k < imgBarriersCount; ++k)
				{
					var current = pImageMemoryBarriers[k];
					var bImage = (VkImage) current.Image;
					Debug.Assert(bImage != null);

                    pImgBarriers[k] = new VkImageMemoryBarrier
					{
						sType = VkStructureType.StructureTypeImageMemoryBarrier,
						pNext = IntPtr.Zero,
						dstAccessMask = (VkAccessFlags)current.DstAccessMask,
						srcAccessMask = (VkAccessFlags)current.SrcAccessMask,
						oldLayout = (VkImageLayout)current.OldLayout,
						newLayout = (VkImageLayout)current.NewLayout,
						srcQueueFamilyIndex = current.SrcQueueFamilyIndex,
						dstQueueFamilyIndex = current.DstQueueFamilyIndex,
						image = bImage.Handle,
						subresourceRange = new VkImageSubresourceRange
						{
							aspectMask = (VkImageAspectFlags)current.SubresourceRange.AspectMask,
							baseArrayLayer = current.SubresourceRange.BaseArrayLayer,
							baseMipLevel = current.SubresourceRange.BaseMipLevel,
							layerCount = current.SubresourceRange.LayerCount,
							levelCount = current.SubresourceRange.LevelCount,
						}
					};
				}

                VkMemoryBarrier* mems = memBarrierCount > 0 ? pMemBarriers : null;
                VkBufferMemoryBarrier* bufs = bufBarrierCount > 0 ? pBufBarriers : null;
                VkImageMemoryBarrier* images = imgBarriersCount > 0 ? pImgBarriers : null;


                Interops.vkCmdPipelineBarrier(Handle,
                      (VkPipelineStageFlags)srcStageMask,
                      (VkPipelineStageFlags)dstStageMask,
                      (VkDependencyFlags)dependencyFlags,
                      memBarrierCount,
                      mems, 
                      bufBarrierCount,
                      bufs,
                      imgBarriersCount,
                      images);
		  	}
		}

		public void CmdBeginQuery(IMgQueryPool queryPool, UInt32 query, MgQueryControlFlagBits flags)
		{
			var bQueryPool = (VkQueryPool) queryPool;
			Debug.Assert(bQueryPool != null);

			Interops.vkCmdBeginQuery(this.Handle, bQueryPool.Handle, query, (Magnesium.Vulkan.VkQueryControlFlags)flags);
		}

		public void CmdEndQuery(IMgQueryPool queryPool, UInt32 query)
		{
			var bQueryPool = (VkQueryPool) queryPool;
			Debug.Assert(bQueryPool != null);

			Interops.vkCmdEndQuery(this.Handle, bQueryPool.Handle, query);
		}

		public void CmdResetQueryPool(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount)
		{
			var bQueryPool = (VkQueryPool) queryPool;
			Debug.Assert(bQueryPool != null);

			Interops.vkCmdResetQueryPool(this.Handle, bQueryPool.Handle, firstQuery, queryCount);
		}

		public void CmdWriteTimestamp(MgPipelineStageFlagBits pipelineStage, IMgQueryPool queryPool, UInt32 query)
		{
			var bQueryPool = (VkQueryPool) queryPool;
			Debug.Assert(bQueryPool != null);

			Interops.vkCmdWriteTimestamp(this.Handle, (VkPipelineStageFlags)pipelineStage, bQueryPool.Handle, query);
		}

		public void CmdCopyQueryPoolResults(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags)
		{
			var bQueryPool =(VkQueryPool) queryPool;
			Debug.Assert(bQueryPool != null);

			var bDstBuffer = (VkBuffer) dstBuffer;
			Debug.Assert(bDstBuffer != null);

			Interops.vkCmdCopyQueryPoolResults(this.Handle, bQueryPool.Handle, firstQuery, queryCount, bDstBuffer.Handle, dstOffset, stride, (Magnesium.Vulkan.VkQueryResultFlags)flags);
		}

		public void CmdPushConstants(IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, UInt32 offset, UInt32 size, IntPtr pValues)
		{
			var bLayout = (VkPipelineLayout) layout;
			Debug.Assert(bLayout != null);

			Interops.vkCmdPushConstants(this.Handle, bLayout.Handle, (Magnesium.Vulkan.VkShaderStageFlags)stageFlags, offset, size, pValues);
		}

		public void CmdBeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
		{
			if (pRenderPassBegin == null)
			{
				throw new ArgumentNullException(nameof(pRenderPassBegin));
			}

			var bRenderPass = (VkRenderPass) pRenderPassBegin.RenderPass;
			Debug.Assert(bRenderPass != null);
			var bFrameBuffer = (VkFramebuffer) pRenderPassBegin.Framebuffer;
			Debug.Assert(bFrameBuffer != null);

			var clearValueCount = pRenderPassBegin.ClearValues != null ? (UInt32) pRenderPassBegin.ClearValues.Length : 0U;

			var clearValues = IntPtr.Zero;

			try
			{
				if (clearValueCount > 0)
				{
					var stride = Marshal.SizeOf(typeof(MgClearValue));
					clearValues = Marshal.AllocHGlobal((int)(stride * clearValueCount));

					for (uint i = 0; i < clearValueCount; ++i)
					{
						IntPtr dest = IntPtr.Add(clearValues, (int)(i * stride));
						Marshal.StructureToPtr(pRenderPassBegin.ClearValues[i], dest, false);
					}
				}

				var beginInfo = new VkRenderPassBeginInfo
				{
					sType = VkStructureType.StructureTypeRenderPassBeginInfo,
					pNext = IntPtr.Zero,
					renderPass = bRenderPass.Handle,
					framebuffer = bFrameBuffer.Handle,
					renderArea = pRenderPassBegin.RenderArea,
					clearValueCount = clearValueCount,
					pClearValues = clearValues,
				};

				Interops.vkCmdBeginRenderPass(this.Handle, ref beginInfo, (Magnesium.Vulkan.VkSubpassContents)contents);
			}
			finally
			{
				if (clearValues != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(clearValues);
				}
			}
		}

		public void CmdNextSubpass(MgSubpassContents contents)
		{
			Interops.vkCmdNextSubpass(this.Handle, (VkSubpassContents)contents);
		}

		public void CmdEndRenderPass()
		{
			Interops.vkCmdEndRenderPass(this.Handle);
		}

		public void CmdExecuteCommands(IMgCommandBuffer[] pCommandBuffers)
		{
			var handles = new IntPtr[pCommandBuffers.Length];

			var bufferCount = (uint)pCommandBuffers.Length;
			for (uint i = 0; i < bufferCount; ++i)
			{
				var bBuffer = (VkCommandBuffer) pCommandBuffers[i];
				Debug.Assert(bBuffer != null);
				handles[i] = bBuffer.Handle;
			}

			Interops.vkCmdExecuteCommands(this.Handle, bufferCount, handles);
		}

	}
}
