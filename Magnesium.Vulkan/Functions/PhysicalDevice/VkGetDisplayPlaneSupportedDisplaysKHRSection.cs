using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetDisplayPlaneSupportedDisplaysKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetDisplayPlaneSupportedDisplaysKHR(IntPtr physicalDevice, UInt32 planeIndex, ref UInt32 pDisplayCount, UInt64[] pDisplays);

		public static MgResult GetDisplayPlaneSupportedDisplaysKHR(VkPhysicalDeviceInfo info, UInt32 planeIndex, out IMgDisplayKHR[] pDisplays)
		{
			// TODO: add implementation
		}
	}
}
