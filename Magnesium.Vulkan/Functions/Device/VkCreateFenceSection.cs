using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateFenceSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateFence(IntPtr device, [In, Out] VkFenceCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pFence);

		public static MgResult CreateFence(VkDeviceInfo info, MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
		{
			// TODO: add implementation
		}
	}
}
