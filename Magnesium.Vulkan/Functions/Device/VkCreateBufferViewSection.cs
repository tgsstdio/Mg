using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateBufferViewSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateBufferView(IntPtr device, [In, Out] VkBufferViewCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pView);

		public static MgResult CreateBufferView(VkDeviceInfo info, MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
		{
			// TODO: add implementation
		}
	}
}
