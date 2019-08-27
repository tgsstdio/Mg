using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdResetQueryPoolSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdResetQueryPool(IntPtr commandBuffer, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount);

        public static void CmdResetQueryPool(VkCommandBufferInfo info, IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount)
        {
            var bQueryPool = (VkQueryPool)queryPool;
            Debug.Assert(bQueryPool != null);

            vkCmdResetQueryPool(info.Handle, bQueryPool.Handle, firstQuery, queryCount);
        }
    }
}
