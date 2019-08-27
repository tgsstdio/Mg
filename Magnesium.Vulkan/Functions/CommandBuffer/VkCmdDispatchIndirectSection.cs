using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdDispatchIndirectSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdDispatchIndirect(IntPtr commandBuffer, UInt64 buffer, UInt64 offset);

        public static void CmdDispatchIndirect(VkCommandBufferInfo info, IMgBuffer buffer, UInt64 offset)
        {
            var bBuffer = (VkBuffer)buffer;
            Debug.Assert(bBuffer != null);

            vkCmdDispatchIndirect(info.Handle, bBuffer.Handle, offset);
        }
    }
}
