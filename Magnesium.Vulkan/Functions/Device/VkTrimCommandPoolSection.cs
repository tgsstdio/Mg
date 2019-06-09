using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkTrimCommandPoolSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkTrimCommandPool(IntPtr device, UInt64 commandPool, UInt32 flags);

		public static void TrimCommandPool(VkDeviceInfo info, IMgCommandPool commandPool, UInt32 flags)
		{
			// TODO: add implementation
		}
	}
}
