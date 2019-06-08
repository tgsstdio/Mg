using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdResetQueryPoolSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdResetQueryPool(IntPtr commandBuffer, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount);

		public static void CmdResetQueryPool(VkCommandBufferInfo info, IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount)
		{
			// TODO: add implementation
		}
	}
}
