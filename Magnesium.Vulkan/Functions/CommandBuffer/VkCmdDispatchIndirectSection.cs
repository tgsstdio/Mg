using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdDispatchIndirectSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDispatchIndirect(IntPtr commandBuffer, UInt64 buffer, VkDeviceSize offset);

		public static void CmdDispatchIndirect(VkCommandBufferInfo info, IMgBuffer buffer, UInt64 offset)
		{
			// TODO: add implementation
		}
	}
}
