using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkResetCommandBufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkResetCommandBuffer(IntPtr commandBuffer, VkCommandBufferResetFlags flags);

		public static MgResult ResetCommandBuffer(VkCommandBufferInfo info, MgCommandBufferResetFlagBits flags)
		{
			// TODO: add implementation
		}
	}
}
