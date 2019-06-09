using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetMemoryFdKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetMemoryFdKHR(IntPtr device, VkMemoryGetFdInfoKHR pGetFdInfo, ref int pFd);

		public static MgResult GetMemoryFdKHR(VkDeviceInfo info, MgMemoryGetFdInfoKHR pGetFdInfo, ref Int32 pFd)
		{
			// TODO: add implementation
		}
	}
}
