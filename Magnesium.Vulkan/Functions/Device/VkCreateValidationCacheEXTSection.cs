using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateValidationCacheEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateValidationCacheEXT(IntPtr device, [In, Out] VkValidationCacheCreateInfoEXT pCreateInfo, IntPtr pAllocator, ref UInt64 pValidationCache);

		public static MgResult CreateValidationCacheEXT(VkDeviceInfo info, MgValidationCacheCreateInfoEXT pCreateInfo, IMgAllocationCallbacks pAllocator, IMgValidationCacheEXT pValidationCache)
		{
			// TODO: add implementation
		}
	}
}
