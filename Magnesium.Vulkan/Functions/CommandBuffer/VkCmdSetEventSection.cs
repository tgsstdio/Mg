using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdSetEventSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdSetEvent(IntPtr commandBuffer, UInt64 @event, MgPipelineStageFlagBits stageMask);

        public static void CmdSetEvent(VkCommandBufferInfo info, IMgEvent @event, MgPipelineStageFlagBits stageMask)
        {
            var bEvent = (VkEvent)@event;
            Debug.Assert(bEvent != null);

            vkCmdSetEvent(info.Handle, bEvent.Handle, stageMask);
        }
    }
}
