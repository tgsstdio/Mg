using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetExclusiveScissorNVSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetExclusiveScissorNV(IntPtr commandBuffer, UInt32 firstExclusiveScissor, UInt32 exclusiveScissorCount, MgRect2D* pExclusiveScissors);

		public static void CmdSetExclusiveScissorNV(VkCommandBufferInfo info, UInt32 firstExclusiveScissor, MgRect2D[] exclusiveScissors)
		{
			// TODO: add implementation
		}
	}
}
