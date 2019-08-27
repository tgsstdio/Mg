using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetSwapchainImagesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetSwapchainImagesKHR(IntPtr device, UInt64 swapchain, ref UInt32 pSwapchainImageCount, [In, Out] UInt64[] pSwapchainImages);

        public static MgResult GetSwapchainImagesKHR(VkDeviceInfo info, IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bSwapchain = (VkSwapchainKHR)swapchain;
            Debug.Assert(bSwapchain != null);

            uint noOfImages = 0;
            var first = vkGetSwapchainImagesKHR(info.Handle, bSwapchain.Handle, ref noOfImages, null);

            if (first != MgResult.SUCCESS)
            {
                pSwapchainImages = null;
                return first;
            }

            var images = new ulong[noOfImages];
            var final = vkGetSwapchainImagesKHR(info.Handle, bSwapchain.Handle, ref noOfImages, images);

            pSwapchainImages = new VkImage[noOfImages];
            for (var i = 0; i < noOfImages; ++i)
            {
                pSwapchainImages[i] = new VkImage(images[i]);
            }

            return final;
        }
    }
}
