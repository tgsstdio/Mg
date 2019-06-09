using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateDescriptorUpdateTemplateSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateDescriptorUpdateTemplate(IntPtr device, [In, Out] VkDescriptorUpdateTemplateCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pDescriptorUpdateTemplate);

		public static MgResult CreateDescriptorUpdateTemplate(VkDeviceInfo info, MgDescriptorUpdateTemplateCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgDescriptorUpdateTemplate pDescriptorUpdateTemplate)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
