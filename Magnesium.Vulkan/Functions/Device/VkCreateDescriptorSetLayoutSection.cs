using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateDescriptorSetLayoutSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateDescriptorSetLayout(IntPtr device, [In, Out] VkDescriptorSetLayoutCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSetLayout);

		public static MgResult CreateDescriptorSetLayout(VkDeviceInfo info, MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
		{
			// TODO: add implementation
		}
	}
}
