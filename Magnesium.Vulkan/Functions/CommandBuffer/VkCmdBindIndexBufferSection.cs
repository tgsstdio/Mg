using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBindIndexBufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdBindIndexBuffer(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, MgIndexType indexType);

        public static void CmdBindIndexBuffer(VkCommandBufferInfo info, IMgBuffer buffer, UInt64 offset, MgIndexType indexType)
        {
            var bBuffer = (VkBuffer)buffer;
            Debug.Assert(bBuffer != null);

            vkCmdBindIndexBuffer(info.Handle, bBuffer.Handle, offset, indexType);
        }
    }
}
