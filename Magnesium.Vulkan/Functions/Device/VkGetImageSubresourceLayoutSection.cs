using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetImageSubresourceLayoutSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetImageSubresourceLayout(IntPtr device, UInt64 image, [In, Out] VkImageSubresource pSubresource, VkSubresourceLayout pLayout);

		public static void GetImageSubresourceLayout(VkDeviceInfo info, IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
		{
			// TODO: add implementation
		}
	}
}
