using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdUpdateBufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdUpdateBuffer(IntPtr commandBuffer, UInt64 dstBuffer, UInt64 dstOffset, UInt64 dataSize, IntPtr pData);

        public static void CmdUpdateBuffer(VkCommandBufferInfo info, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 dataSize, IntPtr pData)
        {
            var bDstBuffer = (VkBuffer)dstBuffer;
            Debug.Assert(bDstBuffer != null);

            vkCmdUpdateBuffer(info.Handle, bDstBuffer.Handle, dstOffset, dataSize, pData);
        }
    }
}
