using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkEndCommandBufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkEndCommandBuffer(IntPtr commandBuffer);

		public static MgResult EndCommandBuffer(VkCommandBufferInfo info)
		{
			// TODO: add implementation
		}
	}
}
