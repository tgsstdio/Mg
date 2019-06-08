using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkRegisterDeviceEventEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkRegisterDeviceEventEXT(IntPtr device, VkDeviceEventInfoEXT pDeviceEventInfo, IntPtr pAllocator, UInt64* pFence);

		public static MgResult RegisterDeviceEventEXT(VkDeviceInfo info, MgDeviceEventInfoEXT pDeviceEventInfo, IntPtr pAllocator, IMgFence pFence)
		{
			// TODO: add implementation
		}
	}
}
