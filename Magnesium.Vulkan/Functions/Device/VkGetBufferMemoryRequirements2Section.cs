using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetBufferMemoryRequirements2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetBufferMemoryRequirements2(IntPtr device, MgBufferMemoryRequirementsInfo2 pInfo, [In, Out] MgMemoryRequirements2 pMemoryRequirements);

		public static void GetBufferMemoryRequirements2(VkDeviceInfo info, MgBufferMemoryRequirementsInfo2 pInfo, out MgMemoryRequirements2 pMemoryRequirements)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
