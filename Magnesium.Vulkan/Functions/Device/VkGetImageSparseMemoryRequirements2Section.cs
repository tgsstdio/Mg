using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetImageSparseMemoryRequirements2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetImageSparseMemoryRequirements2(IntPtr device, VkImageSparseMemoryRequirementsInfo2 pInfo, ref UInt32 pSparseMemoryRequirementCount, [In, Out] VkSparseImageMemoryRequirements2[] pSparseMemoryRequirements);

		public static void GetImageSparseMemoryRequirements2(VkDeviceInfo info, MgImageSparseMemoryRequirementsInfo2 pInfo, out MgSparseImageMemoryRequirements2[] pSparseMemoryRequirements)
		{
			// TODO: add implementation
		}
	}
}
