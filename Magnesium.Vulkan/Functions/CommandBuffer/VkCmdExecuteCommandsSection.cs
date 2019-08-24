using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdExecuteCommandsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdExecuteCommands(IntPtr commandBuffer, UInt32 commandBufferCount, [In] IntPtr[] pCommandBuffers);

        public static void CmdExecuteCommands(VkCommandBufferInfo info, IMgCommandBuffer[] pCommandBuffers)
        {
            var handles = new IntPtr[pCommandBuffers.Length];

            var bufferCount = (uint)pCommandBuffers.Length;
            for (uint i = 0; i < bufferCount; ++i)
            {
                var bBuffer = (VkCommandBuffer)pCommandBuffers[i];
                Debug.Assert(bBuffer != null);
                handles[i] = bBuffer.Info.Handle;
            }

            vkCmdExecuteCommands(info.Handle, bufferCount, handles);
        }
    }
}
