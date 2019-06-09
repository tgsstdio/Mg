using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkImportFenceFdKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkImportFenceFdKHR(IntPtr device, [In, Out] VkImportFenceFdInfoKHR pImportFenceFdInfo);

		public static MgResult ImportFenceFdKHR(VkDeviceInfo info, MgImportFenceFdInfoKHR pImportFenceFdInfo)
		{
			// TODO: add implementation
		}
	}
}
