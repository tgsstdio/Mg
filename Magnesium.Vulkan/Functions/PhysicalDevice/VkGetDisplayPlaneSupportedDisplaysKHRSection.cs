using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetDisplayPlaneSupportedDisplaysKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetDisplayPlaneSupportedDisplaysKHR(IntPtr physicalDevice, UInt32 planeIndex, ref UInt32 pDisplayCount, [In, Out] UInt64[] pDisplays);

        public static MgResult GetDisplayPlaneSupportedDisplaysKHR(VkPhysicalDeviceInfo info, UInt32 planeIndex, out IMgDisplayKHR[] pDisplays)
        {
            uint count = 0;
            var first = vkGetDisplayPlaneSupportedDisplaysKHR(info.Handle, planeIndex, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pDisplays = null;
                return first;
            }

            var supportedDisplays = new ulong[count];
            var final = vkGetDisplayPlaneSupportedDisplaysKHR(info.Handle, planeIndex, ref count, supportedDisplays);

            pDisplays = new VkDisplayKHR[count];
            for (var i = 0; i < count; ++i)
            {
                pDisplays[i] = new VkDisplayKHR(supportedDisplays[i]);
            }

            return final;
        }
    }
}
