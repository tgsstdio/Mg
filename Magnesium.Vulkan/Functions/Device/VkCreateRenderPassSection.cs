using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateRenderPassSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateRenderPass(IntPtr device, [In, Out] VkRenderPassCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pRenderPass);

		public static MgResult CreateRenderPass(VkDeviceInfo info, MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
