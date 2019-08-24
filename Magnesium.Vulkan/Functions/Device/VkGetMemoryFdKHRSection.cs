using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetMemoryFdKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetMemoryFdKHR(IntPtr device, MgMemoryGetFdInfoKHR pGetFdInfo, ref int pFd);

		public static MgResult GetMemoryFdKHR(VkDeviceInfo info, MgMemoryGetFdInfoKHR pGetFdInfo, ref Int32 pFd)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
