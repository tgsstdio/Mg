using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkReleaseDisplayEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkReleaseDisplayEXT(IntPtr physicalDevice, UInt64 display);

		public static MgResult ReleaseDisplayEXT(VkPhysicalDeviceInfo info, IMgDisplayKHR display)
		{
			// TODO: add implementation
		}
	}
}
