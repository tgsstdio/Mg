using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetCalibratedTimestampsEXTSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkGetCalibratedTimestampsEXT(IntPtr device, UInt32 timestampCount, VkCalibratedTimestampInfoEXT* pTimestampInfos, UInt64* pTimestamps, UInt64* pMaxDeviation);

		public static MgResult GetCalibratedTimestampsEXT(VkDeviceInfo info, MgCalibratedTimestampInfoEXT[] pTimestampInfos, out UInt64[] pTimestamps, out UInt64 pMaxDeviation)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
