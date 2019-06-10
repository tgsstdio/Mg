using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkEnumeratePhysicalDeviceGroupsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static unsafe MgResult vkEnumeratePhysicalDeviceGroups(IntPtr instance, ref UInt32 pPhysicalDeviceGroupCount, [In, Out] VkPhysicalDeviceGroupProperties[] pPhysicalDeviceGroupProperties);

        public static MgResult EnumeratePhysicalDeviceGroups(VkInstanceInfo info, out MgPhysicalDeviceGroupProperties[] pPhysicalDeviceGroupProperties)
        {
            Debug.Assert(!info.IsDisposed);

            var count = 0U;

            var first = vkEnumeratePhysicalDeviceGroups(info.Handle, ref count, null);

            var srcGroups = new VkPhysicalDeviceGroupProperties[count];
            var final = vkEnumeratePhysicalDeviceGroups(info.Handle, ref count, srcGroups);

            var dstGroups = new MgPhysicalDeviceGroupProperties[count];
            for (var i = 0; i < count; ++i)
            {
                var deviceCount = srcGroups[i].physicalDeviceCount;

                var devices = new IMgPhysicalDevice[deviceCount];

                for (var j = 0; j < deviceCount; j += 1)
                {
                    devices[j] = new VkPhysicalDevice(srcGroups[i].physicalDevices[j]);
                }

                dstGroups[i] = new MgPhysicalDeviceGroupProperties
                {
                    PhysicalDevices = devices,
                    SubsetAllocation = VkBool32.ConvertFrom(srcGroups[i].subsetAllocation),
                };
            }

            pPhysicalDeviceGroupProperties = dstGroups;

            return final;
        }
    }
}
