using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkDisplayPowerControlEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkDisplayPowerControlEXT(IntPtr device, UInt64 display, VkDisplayPowerInfoEXT pDisplayPowerInfo);

		public static MgResult DisplayPowerControlEXT(VkDeviceInfo info, IMgDisplayKHR display, out MgDisplayPowerInfoEXT pDisplayPowerInfo)
		{
			// TODO: add implementation
		}
	}
}
