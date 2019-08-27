using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetSemaphoreFdKHRSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetSemaphoreFdKHR(IntPtr device, VkSemaphoreGetFdInfoKHR pGetFdInfo, ref int pFd);

		public static MgResult GetSemaphoreFdKHR(VkDeviceInfo info, MgSemaphoreGetFdInfoKHR pGetFdInfo, ref Int32 pFd)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
