using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateSamplerYcbcrConversionSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkCreateSamplerYcbcrConversion(IntPtr device, VkSamplerYcbcrConversionCreateInfo pCreateInfo, IntPtr pAllocator, UInt64* pYcbcrConversion);

		public static MgResult CreateSamplerYcbcrConversion(VkDeviceInfo info, MgSamplerYcbcrConversionCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, IMgSamplerYcbcrConversion pYcbcrConversion)
		{
			// TODO: add implementation
		}
	}
}
