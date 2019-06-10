using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateImageSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateImage(IntPtr device, ref VkImageCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pImage);

        public static MgResult CreateImage(VkDeviceInfo info, MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            uint queueFamilyIndexCount = 0;
            var pQueueFamilyIndices = IntPtr.Zero;

            try
            {
                if (pCreateInfo.QueueFamilyIndices != null)
                {
                    queueFamilyIndexCount = (uint)pCreateInfo.QueueFamilyIndices.Length;
                    pQueueFamilyIndices = VkInteropsUtility.AllocateUInt32Array(pCreateInfo.QueueFamilyIndices);
                }

                ulong internalHandle = 0;

                var createInfo = new VkImageCreateInfo
                {
                    sType = VkStructureType.StructureTypeImageCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    imageType = (VkImageType)pCreateInfo.ImageType,
                    format = pCreateInfo.Format,
                    extent = pCreateInfo.Extent,
                    mipLevels = pCreateInfo.MipLevels,
                    arrayLayers = pCreateInfo.ArrayLayers,
                    samples = pCreateInfo.Samples,
                    tiling = pCreateInfo.Tiling,
                    usage = pCreateInfo.Usage,
                    sharingMode = (VkSharingMode)pCreateInfo.SharingMode,
                    queueFamilyIndexCount = queueFamilyIndexCount,
                    pQueueFamilyIndices = pQueueFamilyIndices,
                    initialLayout = pCreateInfo.InitialLayout,
                };
                var result = vkCreateImage(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pImage = new VkImage(internalHandle);
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
