using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetQueryPoolResultsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetQueryPoolResults(IntPtr device, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, MgQueryResultFlagBits flags);

        public static MgResult GetQueryPoolResults(VkDeviceInfo info, IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, MgQueryResultFlagBits flags)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bQueryPool = (VkQueryPool)queryPool;
            Debug.Assert(bQueryPool != null);

            return vkGetQueryPoolResults(info.Handle, bQueryPool.Handle, firstQuery, queryCount, dataSize, pData, stride, flags);
        }
    }
}
