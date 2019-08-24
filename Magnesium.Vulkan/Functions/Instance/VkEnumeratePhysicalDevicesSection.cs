using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkEnumeratePhysicalDevicesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkEnumeratePhysicalDevices(IntPtr instance, ref UInt32 pPhysicalDeviceCount, [In, Out] IntPtr[] pPhysicalDevices);

        public static MgResult EnumeratePhysicalDevices(VkInstanceInfo info, out IMgPhysicalDevice[] physicalDevices)
        {
            Debug.Assert(!info.IsDisposed);

            var pPropertyCount = 0U;

            var first = vkEnumeratePhysicalDevices(info.Handle, ref pPropertyCount, null);

            if (first != MgResult.SUCCESS)
            {
                physicalDevices = null;
                return first;
            }

            var devices = new IntPtr[pPropertyCount];
            var last = vkEnumeratePhysicalDevices(info.Handle, ref pPropertyCount, devices);

            physicalDevices = new VkPhysicalDevice[pPropertyCount];
            for (uint i = 0; i < pPropertyCount; ++i)
            {
                physicalDevices[i] = new VkPhysicalDevice(devices[i]);
            }
            return last;
        }
    }
}
