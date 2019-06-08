using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetDepthBiasSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetDepthBias(IntPtr commandBuffer, float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);

		public static void CmdSetDepthBias(VkCommandBufferInfo info, float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			// TODO: add implementation
		}
	}
}
