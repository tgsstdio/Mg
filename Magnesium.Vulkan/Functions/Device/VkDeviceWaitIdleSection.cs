using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkDeviceWaitIdleSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkDeviceWaitIdle(IntPtr device);

		public static MgResult DeviceWaitIdle(VkDeviceInfo info)
		{
			// TODO: add implementation
		}
	}
}
