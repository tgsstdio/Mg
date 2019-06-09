using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkImportSemaphoreFdKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkImportSemaphoreFdKHR(IntPtr device, [In, Out] MgImportSemaphoreFdInfoKHR pImportSemaphoreFdInfo);

		public static MgResult ImportSemaphoreFdKHR(VkDeviceInfo info, MgImportSemaphoreFdInfoKHR pImportSemaphoreFdInfo)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
