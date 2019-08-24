using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkEndCommandBufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkEndCommandBuffer(IntPtr commandBuffer);

        public static MgResult EndCommandBuffer(VkCommandBufferInfo info)
        {
            return vkEndCommandBuffer(info.Handle);
        }
    }
}
