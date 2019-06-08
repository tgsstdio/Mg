using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBeginQuerySection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginQuery(IntPtr commandBuffer, UInt64 queryPool, UInt32 query, VkQueryControlFlags flags);

		public static void CmdBeginQuery(VkCommandBufferInfo info, IMgQueryPool queryPool, UInt32 query, MgQueryControlFlagBits flags)
		{
			// TODO: add implementation
		}
	}
}
