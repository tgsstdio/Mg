using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdFillBufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdFillBuffer(IntPtr commandBuffer, UInt64 dstBuffer, VkDeviceSize dstOffset, VkDeviceSize size, UInt32 data);

		public static void CmdFillBuffer(VkCommandBufferInfo info, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data)
		{
			// TODO: add implementation
		}
	}
}
