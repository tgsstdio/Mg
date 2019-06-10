using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkSetDebugUtilsObjectNameEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkSetDebugUtilsObjectNameEXT(IntPtr device, [In, Out] VkDebugUtilsObjectNameInfoEXT pNameInfo);

		public static MgResult SetDebugUtilsObjectNameEXT(VkDeviceInfo info, MgDebugUtilsObjectNameInfoEXT pNameInfo)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
