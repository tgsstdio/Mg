using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetImageMemoryRequirements2Section
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static void vkGetImageMemoryRequirements2(IntPtr device, VkImageMemoryRequirementsInfo2 pInfo, [In, Out] VkMemoryRequirements2 pMemoryRequirements);

		public static void GetImageMemoryRequirements2(VkDeviceInfo info, MgImageMemoryRequirementsInfo2 pInfo, out MgMemoryRequirements2 pMemoryRequirements)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
