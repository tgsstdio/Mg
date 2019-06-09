using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkAllocateCommandBuffersSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkAllocateCommandBuffers(IntPtr device, VkCommandBufferAllocateInfo pAllocateInfo, IntPtr* pCommandBuffers);

		public static MgResult AllocateCommandBuffers(VkDeviceInfo info, MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
