using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkDisplayPowerControlEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkDisplayPowerControlEXT(IntPtr device, UInt64 display, MgDisplayPowerInfoEXT pDisplayPowerInfo);

		public static MgResult DisplayPowerControlEXT(VkDeviceInfo info, IMgDisplayKHR display, out MgDisplayPowerInfoEXT pDisplayPowerInfo)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
