using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkTrimCommandPoolSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkTrimCommandPool(IntPtr device, UInt64 commandPool, UInt32 flags);

		public static void TrimCommandPool(VkDeviceInfo info, IMgCommandPool commandPool, UInt32 flags)
		{
			// TODO: add implementation
		}
	}
}
