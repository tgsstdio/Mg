using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetFenceFdKHRSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetFenceFdKHR(IntPtr device, VkFenceGetFdInfoKHR pGetFdInfo, ref int pFd);

		public static MgResult GetFenceFdKHR(VkDeviceInfo info, MgFenceGetFdInfoKHR pGetFdInfo, out Int32 pFd)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
