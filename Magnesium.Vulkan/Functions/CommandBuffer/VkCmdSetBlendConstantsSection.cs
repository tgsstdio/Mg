using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetBlendConstantsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetBlendConstants(IntPtr commandBuffer, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] float[] blendConstants);

		public static void CmdSetBlendConstants(VkCommandBufferInfo info, MgColor4f blendConstants)
		{
			// TODO: add implementation
		}
	}
}
