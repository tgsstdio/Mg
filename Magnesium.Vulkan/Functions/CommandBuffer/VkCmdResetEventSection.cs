using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdResetEventSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdResetEvent(IntPtr commandBuffer, UInt64 @event, MgPipelineStageFlagBits stageMask);

        public static void CmdResetEvent(VkCommandBufferInfo info, IMgEvent @event, MgPipelineStageFlagBits stageMask)
        {
            var bEvent = (VkEvent)@event;
            Debug.Assert(bEvent != null);

            vkCmdResetEvent(info.Handle, bEvent.Handle, stageMask);
        }
    }
}
