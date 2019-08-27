using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetDescriptorSetLayoutSupportSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static void vkGetDescriptorSetLayoutSupport(IntPtr device, [In, Out] VkDescriptorSetLayoutCreateInfo pCreateInfo, VkDescriptorSetLayoutSupport pSupport);

		public static void GetDescriptorSetLayoutSupport(VkDeviceInfo info, MgDescriptorSetLayoutCreateInfo pCreateInfo, out MgDescriptorSetLayoutSupport pSupport)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
