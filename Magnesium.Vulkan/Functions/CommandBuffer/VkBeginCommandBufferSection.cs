using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkBeginCommandBufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkBeginCommandBuffer(IntPtr commandBuffer, [In, Out] VkCommandBufferBeginInfo pBeginInfo);

		public static MgResult BeginCommandBuffer(VkCommandBufferInfo info, MgCommandBufferBeginInfo pBeginInfo)
		{
			// TODO: add implementation
		}
	}
}
