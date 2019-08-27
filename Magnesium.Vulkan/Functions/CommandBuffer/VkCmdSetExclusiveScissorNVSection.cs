using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdSetExclusiveScissorNVSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdSetExclusiveScissorNV(IntPtr commandBuffer, UInt32 firstExclusiveScissor, UInt32 exclusiveScissorCount, MgRect2D* pExclusiveScissors);

        public static void CmdSetExclusiveScissorNV(VkCommandBufferInfo info, UInt32 firstExclusiveScissor, MgRect2D[] exclusiveScissors)
        {
            unsafe
            {
                var pExclusiveScissors = GCHandle.Alloc(exclusiveScissors, GCHandleType.Pinned);

                try
                {
                    var scissorCount = pExclusiveScissors != null ? (uint)exclusiveScissors.Length : 0U;
                    var pinnedObject = pExclusiveScissors.AddrOfPinnedObject();
                    var bRects = (MgRect2D*)pinnedObject.ToPointer();

                    vkCmdSetExclusiveScissorNV(info.Handle, firstExclusiveScissor, scissorCount, bRects);
                }
                finally
                {
                    pExclusiveScissors.Free();
                }
            }
        }
    }
}
