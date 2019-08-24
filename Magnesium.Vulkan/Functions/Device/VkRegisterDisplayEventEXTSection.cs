using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkRegisterDisplayEventEXTSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkRegisterDisplayEventEXT(IntPtr device, UInt64 display, VkDisplayEventInfoEXT pDisplayEventInfo, IntPtr pAllocator, UInt64* pFence);

		public static MgResult RegisterDisplayEventEXT(VkDeviceInfo info, IMgDisplayKHR display, MgDisplayEventInfoEXT pDisplayEventInfo, IMgAllocationCallbacks pAllocator, IMgFence pFence)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
