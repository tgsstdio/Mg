using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateEventSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateEvent(IntPtr device, [In, Out] VkEventCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pEvent);

		public static MgResult CreateEvent(VkDeviceInfo info, MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event)
		{
			// TODO: add implementation
		}
	}
}
