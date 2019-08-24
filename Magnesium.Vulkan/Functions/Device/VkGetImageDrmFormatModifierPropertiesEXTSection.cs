using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetImageDrmFormatModifierPropertiesEXTSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkGetImageDrmFormatModifierPropertiesEXT(IntPtr device, UInt64 image, VkImageDrmFormatModifierPropertiesEXT pProperties);

		public static MgResult GetImageDrmFormatModifierPropertiesEXT(VkDeviceInfo info, IMgImage image, out MgImageDrmFormatModifierPropertiesEXT pProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
