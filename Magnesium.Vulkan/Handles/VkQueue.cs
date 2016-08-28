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

		public Result QueueSubmit(MgSubmitInfo[] pSubmits, IMgFence fence)
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

								pWaitDstStageMask = Marshal.AllocHGlobal((int)(waitSemaphoreCount * sizeof(VkPipelineStageFlags)));
								attachedItems.Add(pWaitDstStageMask);

								var flagStride = sizeof(VkPipelineStageFlags);
								var offset = 0;
								foreach (var ws in currentInfo.WaitSemaphores)
								{
									var mask = ws.WaitDstStageMask;

									var dest = IntPtr.Add(pWaitDstStageMask, offset);
									Marshal.StructureToPtr(mask, dest, false);
									offset += flagStride;
								}
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
									currentInfo.WaitSemaphores,
									(arg) =>
									{
										var bSemaphore = (VkSemaphore)arg.WaitSemaphore;
										Debug.Assert(bSemaphore != null);
										return bSemaphore.Handle;
									});
								attachedItems.Add(pWaitSemaphores);
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

		public Result QueueWaitIdle()
		{
			return Interops.vkQueueWaitIdle(Handle);
		}

		public Result QueueBindSparse(MgBindSparseInfo[] pBindInfo, IMgFence fence)
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
						var waitSemaphoreCount = 0U;
						var pWaitSemaphores = IntPtr.Zero;
						// TODO : stuff here 

						var bufferBindCount = 0U;
						var pBufferBinds = IntPtr.Zero;
						// TODO : stuff here 

						var imageOpaqueBindCount = 0U;
						var pImageOpaqueBinds = IntPtr.Zero;
						// TODO : stuff here 

						var imageBindCount = 0U;
						var pImageBinds = IntPtr.Zero;
						// TODO : stuff here 

						var signalSemaphoreCount = 0U;
						var pSignalSemaphores = IntPtr.Zero;
						// TODO : stuff here 

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

		public Result QueuePresentKHR(MgPresentInfoKHR pPresentInfo)
		{
			throw new NotImplementedException();
		}

	}
}
