using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkBindImageMemory2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkBindImageMemory2(IntPtr device, UInt32 bindInfoCount, VkBindImageMemoryInfo* pBindInfos);

		public static MgResult BindImageMemory2(VkDeviceInfo info, MgBindImageMemoryInfo[] pBindInfos)
		{
			// TODO: add implementation
		}
	}
}
