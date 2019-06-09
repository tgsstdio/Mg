using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkSetDebugUtilsObjectTagEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkSetDebugUtilsObjectTagEXT(IntPtr device, VkDebugUtilsObjectTagInfoEXT pTagInfo);

		public static MgResult SetDebugUtilsObjectTagEXT(VkDeviceInfo info, MgDebugUtilsObjectTagInfoEXT pTagInfo)
		{
			// TODO: add implementation
		}
	}
}
