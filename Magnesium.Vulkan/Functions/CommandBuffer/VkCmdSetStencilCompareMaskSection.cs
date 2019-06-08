using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetStencilCompareMaskSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetStencilCompareMask(IntPtr commandBuffer, VkStencilFaceFlags faceMask, UInt32 compareMask);

		public static void CmdSetStencilCompareMask(VkCommandBufferInfo info, MgStencilFaceFlagBits faceMask, UInt32 compareMask)
		{
			// TODO: add implementation
		}
	}
}
