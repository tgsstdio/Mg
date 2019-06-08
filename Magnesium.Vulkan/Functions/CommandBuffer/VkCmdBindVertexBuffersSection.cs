using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBindVertexBuffersSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindVertexBuffers(IntPtr commandBuffer, UInt32 firstBinding, UInt32 bindingCount, UInt64[] pBuffers, VkDeviceSize[] pOffsets);

		public static void CmdBindVertexBuffers(VkCommandBufferInfo info, UInt32 firstBinding, IMgBuffer[] pBuffers, UInt64[] pOffsets)
		{
			// TODO: add implementation
		}
	}
}
