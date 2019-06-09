using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceCalibrateableTimeDomainsEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceCalibrateableTimeDomainsEXT(IntPtr physicalDevice, ref UInt32 pTimeDomainCount, VkTimeDomainEXT[] pTimeDomains);

		public static MgResult GetPhysicalDeviceCalibrateableTimeDomainsEXT(VkPhysicalDeviceInfo info, out MgTimeDomainEXT[] pTimeDomains)
		{
			// TODO: add implementation
			throw new NotImplementedException();
		}
	}
}
