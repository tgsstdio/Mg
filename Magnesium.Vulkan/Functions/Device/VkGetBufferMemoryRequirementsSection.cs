using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetBufferMemoryRequirementsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetBufferMemoryRequirements(IntPtr device, UInt64 buffer, [In, Out] VkMemoryRequirements pMemoryRequirements);

		public static void GetBufferMemoryRequirements(VkDeviceInfo info, IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
			// TODO: add implementation
		}
	}
}
