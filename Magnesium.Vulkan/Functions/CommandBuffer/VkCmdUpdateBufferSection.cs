using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdUpdateBufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdUpdateBuffer(IntPtr commandBuffer, UInt64 dstBuffer, UInt64 dstOffset, UInt64 dataSize, IntPtr[] pData);

		public static void CmdUpdateBuffer(VkCommandBufferInfo info, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 dataSize, IntPtr pData)
		{
			// TODO: add implementation
		}
	}
}
