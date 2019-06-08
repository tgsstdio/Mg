using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetImageSparseMemoryRequirementsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetImageSparseMemoryRequirements(IntPtr device, UInt64 image, ref UInt32 pSparseMemoryRequirementCount, [In, Out] VkSparseImageMemoryRequirements[] pSparseMemoryRequirements);

		public static void GetImageSparseMemoryRequirements(VkDeviceInfo info, IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
		{
			// TODO: add implementation
		}
	}
}
