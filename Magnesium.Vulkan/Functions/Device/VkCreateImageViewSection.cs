using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateImageViewSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateImageView(IntPtr device, ref VkImageViewCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pView);

        public static MgResult CreateImageView(VkDeviceInfo info, MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var bImage = (VkImage)pCreateInfo.Image;
            Debug.Assert(bImage != null);


            var createInfo = new VkImageViewCreateInfo
            {
                sType = VkStructureType.StructureTypeImageViewCreateInfo,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
                image = bImage.Handle,
                viewType = (VkImageViewType)pCreateInfo.ViewType,
                format = pCreateInfo.Format,
                components = new VkComponentMapping
                {
                    r = (VkComponentSwizzle)pCreateInfo.Components.R,
                    g = (VkComponentSwizzle)pCreateInfo.Components.G,
                    b = (VkComponentSwizzle)pCreateInfo.Components.B,
                    a = (VkComponentSwizzle)pCreateInfo.Components.A,
                },
                subresourceRange = pCreateInfo.SubresourceRange,
            };
            ulong internalHandle = 0;
            var result = vkCreateImageView(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
            pView = new VkImageView(internalHandle);
            return result;
        }
    }
}
