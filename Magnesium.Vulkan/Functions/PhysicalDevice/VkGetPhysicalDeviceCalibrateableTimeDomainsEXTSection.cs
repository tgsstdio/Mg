using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceCalibrateableTimeDomainsEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceCalibrateableTimeDomainsEXT(IntPtr physicalDevice, ref UInt32 pTimeDomainCount, [In, Out] MgTimeDomainEXT[] pTimeDomains);

        public static MgResult GetPhysicalDeviceCalibrateableTimeDomainsEXT(VkPhysicalDeviceInfo info, out MgTimeDomainEXT[] pTimeDomains)
        {
            var pTimeDomainCount = 0U;
            var result = vkGetPhysicalDeviceCalibrateableTimeDomainsEXT(info.Handle, ref pTimeDomainCount, null);

            if (result != MgResult.SUCCESS)
            {
                pTimeDomains = new MgTimeDomainEXT[0];
                return result;
            }

            pTimeDomains = new MgTimeDomainEXT[pTimeDomainCount];
            return vkGetPhysicalDeviceCalibrateableTimeDomainsEXT(info.Handle, ref pTimeDomainCount, pTimeDomains);
        }
    }
}
