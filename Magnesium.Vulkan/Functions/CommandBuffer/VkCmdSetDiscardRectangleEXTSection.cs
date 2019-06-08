using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetDiscardRectangleEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetDiscardRectangleEXT(IntPtr commandBuffer, UInt32 firstDiscardRectangle, UInt32 discardRectangleCount, VkRect2D* pDiscardRectangles);

		public static void CmdSetDiscardRectangleEXT(VkCommandBufferInfo info, UInt32 firstDiscardRectangle, MgRect2D[] discardRectangles)
		{
			// TODO: add implementation
		}
	}
}
