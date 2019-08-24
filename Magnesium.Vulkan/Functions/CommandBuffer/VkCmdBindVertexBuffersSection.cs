using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBindVertexBuffersSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdBindVertexBuffers(IntPtr commandBuffer, UInt32 firstBinding, UInt32 bindingCount, [In] UInt64[] pBuffers, [In] UInt64[] pOffsets);

        public static void CmdBindVertexBuffers(VkCommandBufferInfo info, UInt32 firstBinding, IMgBuffer[] pBuffers, UInt64[] pOffsets)
        {
            var bindingCount = (uint)pBuffers.Length;
            var src = new ulong[pBuffers.Length];
            for (uint i = 0; i < bindingCount; ++i)
            {
                var bBuffer = (VkBuffer)pBuffers[i];
                Debug.Assert(bBuffer != null);
                src[i] = bBuffer.Handle;
            }

            vkCmdBindVertexBuffers(info.Handle, firstBinding, bindingCount, src, pOffsets);
        }
    }
}
