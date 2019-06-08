using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkSetHdrMetadataEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkSetHdrMetadataEXT(IntPtr device, UInt32 swapchainCount, UInt64* pSwapchains, VkHdrMetadataEXT* pMetadata);

		public static void SetHdrMetadataEXT(VkDeviceInfo info, IMgSwapchainKHR[] pSwapchains, MgHdrMetadataEXT pMetadata)
		{
			// TODO: add implementation
		}
	}
}
