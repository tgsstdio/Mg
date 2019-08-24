using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkUpdateDescriptorSetWithTemplateSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkUpdateDescriptorSetWithTemplate(IntPtr device, UInt64 descriptorSet, UInt64 descriptorUpdateTemplate, IntPtr pData);

		public static void UpdateDescriptorSetWithTemplate(VkDeviceInfo info, IMgDescriptorSet descriptorSet, IMgDescriptorUpdateTemplate descriptorUpdateTemplate, IntPtr pData)
		{
			// TODO: add implementation
		}
	}
}
