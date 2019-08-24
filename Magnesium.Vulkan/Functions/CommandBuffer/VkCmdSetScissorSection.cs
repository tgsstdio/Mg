using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetScissorSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdSetScissor(IntPtr commandBuffer, UInt32 firstScissor, UInt32 scissorCount, Magnesium.MgRect2D* pScissors);

        public static void CmdSetScissor(VkCommandBufferInfo info, UInt32 firstScissor, MgRect2D[] pScissors)
        {
            var scissorHandle = GCHandle.Alloc(pScissors, GCHandleType.Pinned);

            try
            {
                unsafe
                {
                    var count = (UInt32)pScissors.Length;
                    var pinnedObject = scissorHandle.AddrOfPinnedObject();

                    var scissors = (MgRect2D*)pinnedObject.ToPointer();

                    vkCmdSetScissor(info.Handle, firstScissor, count, scissors);
                }
            }
            finally
            {
                scissorHandle.Free();
            }
        }

    }
}
