using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBindIndexBufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindIndexBuffer(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, MgIndexType indexType);

		public static void CmdBindIndexBuffer(VkCommandBufferInfo info, IMgBuffer buffer, UInt64 offset, MgIndexType indexType)
		{
			// TODO: add implementation
		}
	}
}
