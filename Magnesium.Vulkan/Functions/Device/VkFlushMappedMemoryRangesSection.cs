using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkFlushMappedMemoryRangesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkFlushMappedMemoryRanges(IntPtr device, UInt32 memoryRangeCount, VkMappedMemoryRange* pMemoryRanges);

		public static MgResult FlushMappedMemoryRanges(VkDeviceInfo info, MgMappedMemoryRange[] pMemoryRanges)
		{
			// TODO: add implementation
		}
	}
}
