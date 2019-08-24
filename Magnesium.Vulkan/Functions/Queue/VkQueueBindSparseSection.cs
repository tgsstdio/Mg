using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueueBindSparseSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe MgResult vkQueueBindSparse(IntPtr queue, UInt32 bindInfoCount, VkBindSparseInfo* pBindInfo, UInt64 fence);

        public static MgResult QueueBindSparse(VkQueueInfo info, MgBindSparseInfo[] pBindInfo, IMgFence fence)
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
                        var pWaitSemaphores = VkQueueUtility.ExtractSemaphores(attachedItems, current.WaitSemaphores, out waitSemaphoreCount);

                        uint bufferBindCount;
                        var pBufferBinds = ExtractBufferBinds(attachedItems, current.BufferBinds, out bufferBindCount);

                        uint imageOpaqueBindCount;
                        var pImageOpaqueBinds = ExtractImageOpaqueBinds(attachedItems, current.ImageOpaqueBinds, out imageOpaqueBindCount);

                        uint imageBindCount;
                        var pImageBinds = ExtractImageBinds(attachedItems, current.ImageBinds, out imageBindCount);

                        uint signalSemaphoreCount;
                        var pSignalSemaphores = VkQueueUtility.ExtractSemaphores(attachedItems, current.SignalSemaphores, out signalSemaphoreCount);

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

                    return vkQueueBindSparse(info.Handle, bindInfoCount, bindInfos, bFencePtr);
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
                                            aspectMask = arg.Subresource.AspectMask,
                                            arrayLayer = arg.Subresource.ArrayLayer,
                                            mipLevel = arg.Subresource.MipLevel,
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
    }
}
