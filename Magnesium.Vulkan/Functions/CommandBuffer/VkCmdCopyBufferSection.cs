using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdCopyBufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdCopyBuffer(IntPtr commandBuffer, UInt64 srcBuffer, UInt64 dstBuffer, UInt32 regionCount, VkBufferCopy* pRegions);

		public static void CmdCopyBuffer(VkCommandBufferInfo info, IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
		{
			// TODO: add implementation
		}
	}
}
