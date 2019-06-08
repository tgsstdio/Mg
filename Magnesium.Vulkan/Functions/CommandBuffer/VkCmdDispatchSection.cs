using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdDispatchSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDispatch(IntPtr commandBuffer, UInt32 groupCountX, UInt32 groupCountY, UInt32 groupCountZ);

		public static void CmdDispatch(VkCommandBufferInfo info, UInt32 x, UInt32 y, UInt32 z)
		{
			// TODO: add implementation
		}
	}
}
