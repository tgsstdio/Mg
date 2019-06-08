using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateBufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateBuffer(IntPtr device, [In, Out] VkBufferCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pBuffer);

		public static MgResult CreateBuffer(VkDeviceInfo info, MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
		{
			// TODO: add implementation
		}
	}
}
