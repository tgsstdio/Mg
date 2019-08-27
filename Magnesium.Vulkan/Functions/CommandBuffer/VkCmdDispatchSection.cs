using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdDispatchSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdDispatch(IntPtr commandBuffer, UInt32 x, UInt32 y, UInt32 z);

        public static void CmdDispatch(VkCommandBufferInfo info, UInt32 x, UInt32 y, UInt32 z)
        {
            vkCmdDispatch(info.Handle, x, y, z);
        }
    }
}
