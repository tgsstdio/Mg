using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetValidationCacheDataEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetValidationCacheDataEXT(IntPtr device, UInt64 validationCache, ref UIntPtr pDataSize, IntPtr[] pData);

		public static MgResult GetValidationCacheDataEXT(VkDeviceInfo info, IMgValidationCacheEXT validationCache, ref UIntPtr pDataSize, IntPtr[] pData)
		{
			// TODO: add implementation
		}
	}
}
