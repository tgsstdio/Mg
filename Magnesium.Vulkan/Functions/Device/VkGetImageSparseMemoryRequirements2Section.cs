using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetImageSparseMemoryRequirements2Section
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetImageSparseMemoryRequirements2(IntPtr device, MgImageSparseMemoryRequirementsInfo2 pInfo, ref UInt32 pSparseMemoryRequirementCount, [In, Out] MgSparseImageMemoryRequirements2[] pSparseMemoryRequirements);

		public static void GetImageSparseMemoryRequirements2(VkDeviceInfo info, MgImageSparseMemoryRequirementsInfo2 pInfo, out MgSparseImageMemoryRequirements2[] pSparseMemoryRequirements)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
