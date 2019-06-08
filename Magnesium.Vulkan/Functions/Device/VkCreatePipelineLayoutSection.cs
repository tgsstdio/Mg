using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreatePipelineLayoutSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreatePipelineLayout(IntPtr device, [In, Out] VkPipelineLayoutCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pPipelineLayout);

		public static MgResult CreatePipelineLayout(VkDeviceInfo info, MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
		{
			// TODO: add implementation
		}
	}
}
