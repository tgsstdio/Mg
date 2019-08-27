using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetDeviceGroupPresentCapabilitiesKHRSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetDeviceGroupPresentCapabilitiesKHR(IntPtr device, [In, Out] VkDeviceGroupPresentCapabilitiesKHR pDeviceGroupPresentCapabilities);

		public static MgResult GetDeviceGroupPresentCapabilitiesKHR(VkDeviceInfo info, out MgDeviceGroupPresentCapabilitiesKHR pDeviceGroupPresentCapabilities)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
