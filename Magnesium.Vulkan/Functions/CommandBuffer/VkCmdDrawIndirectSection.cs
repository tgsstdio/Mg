using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdDrawIndirectSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndirect(IntPtr commandBuffer, UInt64 buffer, VkDeviceSize offset, UInt32 drawCount, UInt32 stride);

		public static void CmdDrawIndirect(VkCommandBufferInfo info, IMgBuffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride)
		{
			// TODO: add implementation
		}
	}
}
