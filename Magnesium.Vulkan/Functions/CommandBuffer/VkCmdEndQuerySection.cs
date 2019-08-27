using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdEndQuerySection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdEndQuery(IntPtr commandBuffer, UInt64 queryPool, UInt32 query);

        public static void CmdEndQuery(VkCommandBufferInfo info, IMgQueryPool queryPool, UInt32 query)
        {
            var bQueryPool = (VkQueryPool)queryPool;
            Debug.Assert(bQueryPool != null);

            vkCmdEndQuery(info.Handle, bQueryPool.Handle, query);
        }
    }
}
