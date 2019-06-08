using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkFreeDescriptorSetsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkFreeDescriptorSets(IntPtr device, UInt64 descriptorPool, UInt32 descriptorSetCount, UInt64[] pDescriptorSets);

		public static MgResult FreeDescriptorSets(VkDeviceInfo info, IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
		{
			// TODO: add implementation
		}
	}
}
