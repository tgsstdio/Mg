using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdDrawIndexedIndirectSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndexedIndirect(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);

		public static void CmdDrawIndexedIndirect(VkCommandBufferInfo info, IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride)
		{
			// TODO: add implementation
		}
	}
}
