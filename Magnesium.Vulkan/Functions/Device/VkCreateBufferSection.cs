using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateBufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateBuffer(IntPtr device, ref VkBufferCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pBuffer);

        public static MgResult CreateBuffer(VkDeviceInfo info, MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            var queueFamilyIndexCount = 0U;
            var pQueueFamilyIndices = IntPtr.Zero;

            try
            {
                if (pCreateInfo.QueueFamilyIndices != null)
                {
                    queueFamilyIndexCount = (uint)pCreateInfo.QueueFamilyIndices.Length;
                    pQueueFamilyIndices = VkInteropsUtility.AllocateUInt32Array(pCreateInfo.QueueFamilyIndices);
                }

                var createInfo = new VkBufferCreateInfo
                {
                    sType = VkStructureType.StructureTypeBufferCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = (VkBufferCreateFlags)pCreateInfo.Flags,
                    sharingMode = (VkSharingMode)pCreateInfo.SharingMode,
                    usage = (VkBufferUsageFlags)pCreateInfo.Usage,
                    size = pCreateInfo.Size,
                    queueFamilyIndexCount = queueFamilyIndexCount,
                    pQueueFamilyIndices = pQueueFamilyIndices,

                };

                var internalHandle = 0UL;
                var result = vkCreateBuffer(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pBuffer = new VkBuffer(internalHandle);
                return result;
            }
            finally
            {
                if (pQueueFamilyIndices != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pQueueFamilyIndices);
                }
            }
        }
    }
}
