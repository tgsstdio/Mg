using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBeginQuerySection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdBeginQuery(IntPtr commandBuffer, UInt64 queryPool, UInt32 query, VkQueryControlFlags flags);

        public static void CmdBeginQuery(VkCommandBufferInfo info, IMgQueryPool queryPool, UInt32 query, MgQueryControlFlagBits flags)
        {
            var bQueryPool = (VkQueryPool)queryPool;
            Debug.Assert(bQueryPool != null);

            vkCmdBeginQuery(info.Handle, bQueryPool.Handle, query, (Magnesium.Vulkan.VkQueryControlFlags)flags);
        }
    }
}
