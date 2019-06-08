using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetLineWidthSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetLineWidth(IntPtr commandBuffer, float lineWidth);

		public static void CmdSetLineWidth(VkCommandBufferInfo info, float lineWidth)
		{
			// TODO: add implementation
		}
	}
}
