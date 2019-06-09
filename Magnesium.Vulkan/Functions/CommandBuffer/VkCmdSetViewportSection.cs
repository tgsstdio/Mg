using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetViewportSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetViewport(IntPtr commandBuffer, UInt32 firstViewport, UInt32 viewportCount, MgViewport* pViewports);

		public static void CmdSetViewport(VkCommandBufferInfo info, UInt32 firstViewport, MgViewport[] pViewports)
		{
			// TODO: add implementation
		}
	}
}
