using Magnesium;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkQueue : IMgQueue
	{
		internal IntPtr Handle = IntPtr.Zero;
		internal VkQueue(IntPtr handle)
		{
			Handle = handle;
		}

		public MgResult QueueSubmit(MgSubmitInfo[] pSubmits, IMgFence fence)
		{
			var bFence = (VkFence)fence;
			var bFencePtr = bFence != null ? bFence.Handle : 0UL;

			var attachedItems = new List<IntPtr>();
			try
			{
				unsafe
				{
					uint submitCount = 0;

					if (pSubmits != null)
					{
						submitCount = (uint)pSubmits.Length;
					}

					var submissions = stackalloc VkSubmitInfo[(int)submitCount];

					for (var i = 0; i < submitCount; ++i)
					{
						var currentInfo = pSubmits[i];

						var waitSemaphoreCount = 0U;
						var pWaitSemaphores = IntPtr.Zero;
						var pWaitDstStageMask = IntPtr.Zero;

						if (currentInfo.WaitSemaphores != null)
						{
							waitSemaphoreCount = (uint)currentInfo.WaitSemaphores.Length;
							if (waitSemaphoreCount > 0)
							{
								pWaitSemaphores = VkInteropsUtility.ExtractUInt64HandleArray(
									currentInfo.WaitSemaphores,
									(arg) =>
									{
										var bSemaphore = (VkSemaphore)arg.WaitSemaphore;
										Debug.Assert(bSemaphore != null);
										return bSemaphore.Handle;
									});
								attachedItems.Add(pWaitSemaphores);

								pWaitDstStageMask = VkInteropsUtility.AllocateHGlobalArray<MgSubmitInfoWaitSemaphoreInfo, uint>(
									currentInfo.WaitSemaphores,
									(arg) => { return (uint) arg.WaitDstStageMask; });
								attachedItems.Add(pWaitDstStageMask);	
							}
						}

						var commandBufferCount = 0U;
						var pCommandBuffers = IntPtr.Zero;

						if (currentInfo.CommandBuffers != null)
						{
							commandBufferCount = (uint)currentInfo.CommandBuffers.Length;
							if (commandBufferCount > 0)
							{
								pCommandBuffers = VkInteropsUtility.ExtractIntPtrHandleArray
                            	(
									currentInfo.CommandBuffers,
									(arg) =>
									{
										var bCommandBuffer = (VkCommandBuffer)arg;
										Debug.Assert(bCommandBuffer != null);
										return bCommandBuffer.Handle;
									}
                              	);
								attachedItems.Add(pCommandBuffers);
							}
						}

						var signalSemaphoreCount = 0U;
						var pSignalSemaphores = IntPtr.Zero;

						if (currentInfo.SignalSemaphores != null)
						{
							signalSemaphoreCount = (uint)currentInfo.SignalSemaphores.Length;

							if (signalSemaphoreCount > 0)
							{
								pSignalSemaphores = VkInteropsUtility.ExtractUInt64HandleArray(
									currentInfo.SignalSemaphores,
									(arg) =>
									{
										var bSemaphore = (VkSemaphore)arg;
										Debug.Assert(bSemaphore != null);
										return bSemaphore.Handle;
									});
								attachedItems.Add(pSignalSemaphores);
							}
						}

						submissions[i] = new VkSubmitInfo
						{
							sType = VkStructureType.StructureTypeSubmitInfo,
							pNext = IntPtr.Zero,
							waitSemaphoreCount = waitSemaphoreCount,
							pWaitSemaphores = pWaitSemaphores,
							pWaitDstStageMask = pWaitDstStageMask,
							commandBufferCount = commandBufferCount,
							pCommandBuffers = pCommandBuffers,
							signalSemaphoreCount = signalSemaphoreCount,
							pSignalSemaphores = pSignalSemaphores,
						};
					}

					return Interops.vkQueueSubmit(Handle, submitCount, submitCount > 0  ? submissions : null, bFencePtr);
				}
			}
			finally
			{
				foreach (var item in attachedItems)
				{
					Marshal.FreeHGlobal(item);
				}
			}
		}

		public MgResult QueueWaitIdle()
		{
			return Interops.vkQueueWaitIdle(Handle);
		}

		public MgResult QueueBindSparse(MgBindSparseInfo[] pBindInfo, IMgFence fence)
		{
			var bFence = (VkFence)fence;
			var bFencePtr = bFence != null ? bFence.Handle : 0UL;

			var attachedItems = new List<IntPtr>();
			try
			{
				unsafe
				{
					var bindInfoCount = 0U;

					var bindInfos = stackalloc VkBindSparseInfo[(int)bindInfoCount];

					for (var i = 0; i < bindInfoCount; ++i)
					{
						var current = pBindInfo[i];

						uint waitSemaphoreCount;
						var pWaitSemaphores = ExtractSemaphores(attachedItems, current.WaitSemaphores, out waitSemaphoreCount);

						uint bufferBindCount;
						var pBufferBinds = ExtractBufferBinds(attachedItems, current.BufferBinds, out bufferBindCount);

						uint imageOpaqueBindCount;
						var pImageOpaqueBinds = ExtractImageOpaqueBinds(attachedItems, current.ImageOpaqueBinds, out imageOpaqueBindCount);

						uint imageBindCount;
						var pImageBinds = ExtractImageBinds(attachedItems, current.ImageBinds, out imageBindCount);

						uint signalSemaphoreCount;
						var pSignalSemaphores = ExtractSemaphores(attachedItems, current.SignalSemaphores, out signalSemaphoreCount);

						bindInfos[i] = new VkBindSparseInfo
						{
							sType = VkStructureType.StructureTypeBindSparseInfo,
							pNext = IntPtr.Zero,
							waitSemaphoreCount = waitSemaphoreCount,
							pWaitSemaphores = pWaitSemaphores,
							bufferBindCount = bufferBindCount,
							pBufferBinds = pBufferBinds,
							imageOpaqueBindCount = imageOpaqueBindCount,
							pImageOpaqueBinds = pImageOpaqueBinds,
							imageBindCount = imageBindCount,
							pImageBinds = pImageBinds,
							signalSemaphoreCount = signalSemaphoreCount,
							pSignalSemaphores = pSignalSemaphores,
						};
					}

					return Interops.vkQueueBindSparse(Handle, bindInfoCount, bindInfos, bFencePtr);
				}
			}
			finally
			{
				foreach (var item in attachedItems)
				{
					Marshal.FreeHGlobal(item);
				}
			}
		}

		static IntPtr ExtractImageBinds(List<IntPtr> attachedItems, MgSparseImageMemoryBindInfo[] imageBinds, out uint imageBindCount)
		{
			var dest = IntPtr.Zero;
			uint count = 0U;

			if (imageBinds != null)
			{
				count = (uint)imageBinds.Length;
				if (count > 0)
				{
					dest = VkInteropsUtility.AllocateNestedHGlobalArray(
						attachedItems,
						imageBinds,
						(items, bind) => 
						{
							var bImage = (VkImage)bind.Image;
							Debug.Assert(bImage != null);

							Debug.Assert(bind.Binds != null);
							var bindCount = (uint)bind.Binds.Length;

							var pBinds = VkInteropsUtility.AllocateHGlobalArray(
								bind.Binds,
								(arg) =>
								{
									var bDeviceMemory = (VkDeviceMemory)arg.Memory;
									Debug.Assert(bDeviceMemory != null);

									return new VkSparseImageMemoryBind
									{
										subresource = new VkImageSubresource
										{
											aspectMask = (VkImageAspectFlags)arg.Subresource.AspectMask,
											arrayLayer = arg.Subresource.ArrayLayer,
											mipLevel= arg.Subresource.MipLevel,
										},
										offset = arg.Offset,
										extent = arg.Extent,
										memory = bDeviceMemory.Handle,
										memoryOffset = arg.MemoryOffset,
										flags = (VkSparseMemoryBindFlags)arg.Flags,
									};
								});
							items.Add(pBinds);

							return new VkSparseImageMemoryBindInfo
							{
								image = bImage.Handle,
								bindCount = bindCount,
								pBinds = pBinds,
							};
						});

					attachedItems.Add(dest);
				}
			}

			imageBindCount = count;
			return dest;
		}

		static IntPtr ExtractImageOpaqueBinds(List<IntPtr> attachedItems, MgSparseImageOpaqueMemoryBindInfo[] imageOpaqueBinds, out uint imageOpaqueBindCount)
		{
			var dest = IntPtr.Zero;
			uint count = 0U;

			if (imageOpaqueBinds != null)
			{
				count = (uint)imageOpaqueBinds.Length;
				if (count > 0)
				{
					dest = VkInteropsUtility.AllocateNestedHGlobalArray(
						attachedItems,
						imageOpaqueBinds,
						(items, bind) =>
						{
							var bImage = (VkImage)bind.Image;
							Debug.Assert(bImage != null);

							Debug.Assert(bind.Binds != null);
							var bindCount = (uint)bind.Binds.Length;
							var pBinds = VkInteropsUtility.AllocateHGlobalArray(
								bind.Binds,
								(arg) =>
								{
									var bDeviceMemory = (VkDeviceMemory)arg.Memory;
									Debug.Assert(bDeviceMemory != null);

									return new VkSparseMemoryBind
									{
										resourceOffset = arg.ResourceOffset,
										size = arg.Size,
										memory = bDeviceMemory.Handle,
										memoryOffset = arg.MemoryOffset,
										flags = (VkSparseMemoryBindFlags)arg.Flags,
									};
								});
							items.Add(pBinds);

							return new VkSparseImageOpaqueMemoryBindInfo
							{
								image = bImage.Handle,
								bindCount = bindCount,
								pBinds = pBinds,
							};
						});
					attachedItems.Add(dest);
				}
			}

			imageOpaqueBindCount = count;
			return dest;
		}

		static IntPtr ExtractBufferBinds(List<IntPtr> attachedItems, MgSparseBufferMemoryBindInfo[] bufferBinds, out uint bufferBindCount)
		{
			var dest = IntPtr.Zero;
			uint count = 0U;

			if (bufferBinds != null)
			{
				count = (uint)bufferBinds.Length;
				if (count > 0)
				{
					dest = VkInteropsUtility.AllocateNestedHGlobalArray(
						attachedItems,
						bufferBinds,
						(items, bind) =>
						{
							var bBuffer = (VkBuffer)bind.Buffer;
							Debug.Assert(bBuffer != null);

							Debug.Assert(bind.Binds != null);
							var bindCount = (uint) bind.Binds.Length;
							var pBinds = VkInteropsUtility.AllocateHGlobalArray(
								bind.Binds,
								(arg) =>
								{
									var bDeviceMemory = (VkDeviceMemory)arg.Memory;
									Debug.Assert(bDeviceMemory != null);

									return new VkSparseMemoryBind
									{
										resourceOffset = arg.ResourceOffset,
										size = arg.Size,
										memory = bDeviceMemory.Handle,
										memoryOffset = arg.MemoryOffset,
										flags = (VkSparseMemoryBindFlags)arg.Flags,
									};
								});
							items.Add(pBinds);
							
							return new VkSparseBufferMemoryBindInfo
							{
								buffer = bBuffer.Handle,
								bindCount = bindCount,
								pBinds = pBinds,
							};
						}
					);
					attachedItems.Add(dest);
				}
			}

			bufferBindCount = count;
			return dest;
		}

		static IntPtr ExtractSemaphores(List<IntPtr> attachedItems, IMgSemaphore[] semaphores, out uint semaphoreCount)
		{
			var dest = IntPtr.Zero;
			uint count = 0U;

			if (semaphores != null)
			{
				semaphoreCount = (uint)semaphores.Length;
				if (semaphoreCount > 0)
				{
					dest = VkInteropsUtility.ExtractUInt64HandleArray(
						semaphores,
						(arg) =>
						{
							var bSemaphore = (VkSemaphore)arg;
							Debug.Assert(bSemaphore != null);
							return bSemaphore.Handle;
						}
					);
					attachedItems.Add(dest);
				}
			}
			semaphoreCount = count;
			return dest;
		}

		public MgResult QueuePresentKHR(MgPresentInfoKHR pPresentInfo)
		{
			if (pPresentInfo == null)
				throw new ArgumentNullException(nameof(pPresentInfo));

			var attachedItems = new List<IntPtr>();
			try
			{
				uint waitSemaphoreCount;
				var pWaitSemaphores = ExtractSemaphores(attachedItems, pPresentInfo.WaitSemaphores, out waitSemaphoreCount);

				IntPtr pSwapchains;
				IntPtr pImageIndices;
				uint swapchainCount = ExtractSwapchains(attachedItems, pPresentInfo.Images, out pSwapchains, out pImageIndices);

				var pResults = ExtractResults(attachedItems, pPresentInfo.Results);

				var presentInfo = new VkPresentInfoKHR
				{
					sType = VkStructureType.StructureTypePresentInfoKhr,
					pNext = IntPtr.Zero,
					waitSemaphoreCount = waitSemaphoreCount,
					pWaitSemaphores = pWaitSemaphores,
					swapchainCount = swapchainCount,
					pSwapchains = pSwapchains,
					pImageIndices = pImageIndices,
					pResults = pResults,
				};

				var result = Interops.vkQueuePresentKHR(Handle, ref presentInfo);

				// MUST ABLE TO RETURN 
				if (pResults != IntPtr.Zero)
				{
					var stride = Marshal.SizeOf(typeof(MgResult));
					var swapChains = new MgResult[swapchainCount];
					var offset = 0;
					for (var i = 0; i < swapchainCount; ++i)
					{
						var src = IntPtr.Add(pResults, offset);
						swapChains[i] = (Magnesium.MgResult)Marshal.PtrToStructure(src, typeof(Magnesium.MgResult));
						offset += stride;
					}

					pPresentInfo.Results = swapChains;
				}

				return result;
			}
			finally
			{
				foreach (var item in attachedItems)
				{
					Marshal.FreeHGlobal(item);
				}
			}
		}

		static IntPtr ExtractResults(List<IntPtr> attachedItems, MgResult[] results)
		{
			if (results == null)
				return IntPtr.Zero;

			var stride = Marshal.SizeOf(typeof(MgResult));
			var dest = Marshal.AllocHGlobal(stride * results.Length);
			attachedItems.Add(dest);
			return dest;
		}

		uint ExtractSwapchains(List<IntPtr> attachedItems, MgPresentInfoKHRImage[] images, out IntPtr swapchains, out IntPtr imageIndices)
		{
			var pSwapchains = IntPtr.Zero;
			var pImageIndices = IntPtr.Zero;
			uint count = 0U;

			if (images != null)
			{
				count = (uint)images.Length;
				if (count > 0)
				{
					pSwapchains = VkInteropsUtility.ExtractUInt64HandleArray(
						images,
						(sc) => 
						{ 
							var bSwapchain = (VkSwapchainKHR)sc.Swapchain;
							Debug.Assert(bSwapchain != null);
							return bSwapchain.Handle;
						});
					attachedItems.Add(pSwapchains);

                    pImageIndices = VkInteropsUtility.AllocateHGlobalArray(
                        images,
                        (sc) => { return sc.ImageIndex; });
                    attachedItems.Add(pImageIndices);
                }
			}
			swapchains = pSwapchains;
			imageIndices = pImageIndices;
			return count;
		}

        public void GetQueueCheckpointDataNV(out MgCheckpointDataNV[] pCheckpointData)
        {
            var pCheckpointDataCount = 0U;

            Interops.vkGetQueueCheckpointDataNV(this.Handle, ref pCheckpointDataCount, null);

            pCheckpointData = new MgCheckpointDataNV[pCheckpointDataCount];
            if (pCheckpointDataCount > 0)
            {
                var bCheckpointData = new VkCheckpointDataNV[pCheckpointDataCount];

                Interops.vkGetQueueCheckpointDataNV(this.Handle, ref pCheckpointDataCount, bCheckpointData);

                for (var i = 0U; i < pCheckpointDataCount; i += 1)
                {
                    var current = bCheckpointData[i];

                    pCheckpointData[i] = new MgCheckpointDataNV
                    {
                        Stage = current.stage,
                        CheckpointMarker = current.pCheckpointMarker,
                    };
                }
            }
        }

        public void QueueBeginDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo)
        {
            var pLabelName = IntPtr.Zero;

            try
            {
                pLabelName = VkInteropsUtility.NativeUtf8FromString(labelInfo.LabelName);

                var lbl = new VkDebugUtilsLabelEXT
                {
                    sType = VkStructureType.StructureTypeDebugUtilsLabelExt,
                    pNext = IntPtr.Zero,
                    pLabelName = pLabelName,
                    color = labelInfo.Color,
                };

                Interops.vkQueueBeginDebugUtilsLabelEXT(this.Handle, ref lbl);
            }
            finally
            {
                Marshal.FreeHGlobal(pLabelName);
            }
        }

        public void QueueInsertDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo)
        {
            var pLabelName = IntPtr.Zero;

            try
            {
                pLabelName = VkInteropsUtility.NativeUtf8FromString(labelInfo.LabelName);

                var lbl = new VkDebugUtilsLabelEXT
                {
                    sType = VkStructureType.StructureTypeDebugUtilsLabelExt,
                    pNext = IntPtr.Zero,
                    pLabelName = pLabelName,
                    color = labelInfo.Color,
                };

                Interops.vkQueueInsertDebugUtilsLabelEXT(this.Handle, ref lbl);
            }
            finally
            {
                Marshal.FreeHGlobal(pLabelName);
            }
        }

        public void QueueEndDebugUtilsLabelEXT()
        {
            Interops.vkQueueEndDebugUtilsLabelEXT(this.Handle);
        }
    }
}
