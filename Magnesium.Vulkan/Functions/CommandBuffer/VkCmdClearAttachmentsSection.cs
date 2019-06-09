using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdClearAttachmentsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdClearAttachments(IntPtr commandBuffer, UInt32 attachmentCount, [In, Out] MgClearAttachment[] pAttachments, UInt32 rectCount, MgClearRect[] pRects);

		public static void CmdClearAttachments(VkCommandBufferInfo info, MgClearAttachment[] pAttachments, MgClearRect[] pRects)
		{
			// TODO: add implementation
		}
	}
}
