using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkFreeCommandBuffersSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkFreeCommandBuffers(IntPtr device, UInt64 commandPool, UInt32 commandBufferCount, IntPtr[] pCommandBuffers);

		public static void FreeCommandBuffers(VkDeviceInfo info, IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
		{
			// TODO: add implementation
		}
	}
}
