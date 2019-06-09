using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateCommandPoolSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateCommandPool(IntPtr device, [In, Out] VkCommandPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pCommandPool);

		public static MgResult CreateCommandPool(VkDeviceInfo info, MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
