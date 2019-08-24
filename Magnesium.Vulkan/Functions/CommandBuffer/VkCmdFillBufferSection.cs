using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdFillBufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdFillBuffer(IntPtr commandBuffer, UInt64 dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data);

        public static void CmdFillBuffer(VkCommandBufferInfo info, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data)
        {
            var bDstBuffer = (VkBuffer)dstBuffer;
            Debug.Assert(bDstBuffer != null);

            vkCmdFillBuffer(info.Handle, bDstBuffer.Handle, dstOffset, size, data);
        }
    }
}
