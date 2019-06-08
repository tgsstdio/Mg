using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkMergeValidationCachesEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkMergeValidationCachesEXT(IntPtr device, UInt64 dstCache, UInt32 srcCacheCount, UInt64[] pSrcCaches);

		public static MgResult MergeValidationCachesEXT(VkDeviceInfo info, IMgValidationCacheEXT dstCache, IMgValidationCacheEXT[] pSrcCaches)
		{
			// TODO: add implementation
		}
	}
}
