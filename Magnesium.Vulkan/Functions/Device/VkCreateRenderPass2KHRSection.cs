using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateRenderPass2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateRenderPass2KHR(IntPtr device, [In, Out] VkRenderPassCreateInfo2KHR pCreateInfo, IntPtr pAllocator, ref UInt64 pRenderPass);

		public static MgResult CreateRenderPass2KHR(VkDeviceInfo info, MgRenderPassCreateInfo2KHR pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgRenderPass pRenderPass)
		{
			// TODO: add implementation
		}
	}
}
