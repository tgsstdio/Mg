using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetImageSubresourceLayoutSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetImageSubresourceLayout(IntPtr device, UInt64 image, [In] Magnesium.MgImageSubresource pSubresource, [In, Out] Magnesium.MgSubresourceLayout pLayout);

        public static void GetImageSubresourceLayout(VkDeviceInfo info, IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bImage = (VkImage)image;
            Debug.Assert(bImage != null);

            var layout = default(MgSubresourceLayout);
            vkGetImageSubresourceLayout(info.Handle, bImage.Handle, pSubresource, layout);
            pLayout = layout;
        }
    }
}
