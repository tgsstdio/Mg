using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkUpdateDescriptorSetsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkUpdateDescriptorSets(IntPtr device, UInt32 descriptorWriteCount, [In, Out] VkWriteDescriptorSet[] pDescriptorWrites, UInt32 descriptorCopyCount, VkCopyDescriptorSet[] pDescriptorCopies);

		public static void UpdateDescriptorSets(VkDeviceInfo info, MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
		{
			// TODO: add implementation
		}
	}
}
