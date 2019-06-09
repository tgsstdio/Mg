using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetMemoryFdPropertiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetMemoryFdPropertiesKHR(IntPtr device, MgExternalMemoryHandleTypeFlagBits handleType, int fd, MgMemoryFdPropertiesKHR pMemoryFdProperties);

		public static MgResult GetMemoryFdPropertiesKHR(VkDeviceInfo info, MgExternalMemoryHandleTypeFlagBits handleType, Int32 fd, out MgMemoryFdPropertiesKHR pMemoryFdProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
