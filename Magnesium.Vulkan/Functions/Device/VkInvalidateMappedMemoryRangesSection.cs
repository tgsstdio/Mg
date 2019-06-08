using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkInvalidateMappedMemoryRangesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkInvalidateMappedMemoryRanges(IntPtr device, UInt32 memoryRangeCount, VkMappedMemoryRange* pMemoryRanges);

		public static MgResult InvalidateMappedMemoryRanges(VkDeviceInfo info, MgMappedMemoryRange[] pMemoryRanges)
		{
			// TODO: add implementation
		}
	}
}
