using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkAllocateMemorySection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkAllocateMemory(IntPtr device, VkMemoryAllocateInfo pAllocateInfo, IntPtr pAllocator, UInt64* pMemory);

		public static MgResult AllocateMemory(VkDeviceInfo info, MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
		{
			// TODO: add implementation
		}
	}
}
