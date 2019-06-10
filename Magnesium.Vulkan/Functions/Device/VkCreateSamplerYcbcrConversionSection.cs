using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateSamplerYcbcrConversionSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkCreateSamplerYcbcrConversion(IntPtr device, MgSamplerYcbcrConversionCreateInfo pCreateInfo, IntPtr pAllocator, UInt64* pYcbcrConversion);

		public static MgResult CreateSamplerYcbcrConversion(VkDeviceInfo info, MgSamplerYcbcrConversionCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, IMgSamplerYcbcrConversion pYcbcrConversion)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
