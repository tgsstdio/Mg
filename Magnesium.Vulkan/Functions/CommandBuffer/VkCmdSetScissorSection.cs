using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetScissorSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetScissor(IntPtr commandBuffer, UInt32 firstScissor, UInt32 scissorCount, MgRect2D* pScissors);

		public static void CmdSetScissor(VkCommandBufferInfo info, UInt32 firstScissor, MgRect2D[] pScissors)
		{
			// TODO: add implementation
		}
	}
}
