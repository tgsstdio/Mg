using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdEndQuerySection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdEndQuery(IntPtr commandBuffer, UInt64 queryPool, UInt32 query);

		public static void CmdEndQuery(VkCommandBufferInfo info, IMgQueryPool queryPool, UInt32 query)
		{
			// TODO: add implementation
		}
	}
}
