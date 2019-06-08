using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetImageMemoryRequirementsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetImageMemoryRequirements(IntPtr device, UInt64 image, [In, Out] VkMemoryRequirements pMemoryRequirements);

		public static void GetImageMemoryRequirements(VkDeviceInfo info, IMgImage image, out MgMemoryRequirements memoryRequirements)
		{
			// TODO: add implementation
		}
	}
}
