using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetMemoryHostPointerPropertiesEXTSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkGetMemoryHostPointerPropertiesEXT(IntPtr device, MgExternalMemoryHandleTypeFlagBits handleType, IntPtr pHostPointer, MgMemoryHostPointerPropertiesEXT* pMemoryHostPointerProperties);

		public static MgResult GetMemoryHostPointerPropertiesEXT(VkDeviceInfo info, MgExternalMemoryHandleTypeFlagBits handleType, IntPtr pHostPointer, out MgMemoryHostPointerPropertiesEXT pMemoryHostPointerProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
