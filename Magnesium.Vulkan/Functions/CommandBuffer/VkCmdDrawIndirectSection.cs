using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdDrawIndirectSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdDrawIndirect(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);

        public static void CmdDrawIndirect(VkCommandBufferInfo info, IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride)
        {
            var bBuffer = (VkBuffer)buffer;
            Debug.Assert(bBuffer != null);

            vkCmdDrawIndirect(info.Handle, bBuffer.Handle, offset, drawCount, stride);
        }
    }
}
