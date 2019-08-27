using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdSetBlendConstantsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdSetBlendConstants(IntPtr commandBuffer, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] float[] blendConstants);

        public static void CmdSetBlendConstants(VkCommandBufferInfo info, MgColor4f blendConstants)
        {
            var color = new float[4];
            color[0] = blendConstants.R;
            color[1] = blendConstants.G;
            color[2] = blendConstants.B;
            color[3] = blendConstants.A;

            // TODO : figure a way to directly pass in
            vkCmdSetBlendConstants(info.Handle, color);
        }
    }
}
