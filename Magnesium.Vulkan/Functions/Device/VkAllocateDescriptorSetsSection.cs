using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkAllocateDescriptorSetsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkAllocateDescriptorSets(IntPtr device, VkDescriptorSetAllocateInfo pAllocateInfo, UInt64* pDescriptorSets);

		public static MgResult AllocateDescriptorSets(VkDeviceInfo info, MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
		{
			// TODO: add implementation
		}
	}
}
