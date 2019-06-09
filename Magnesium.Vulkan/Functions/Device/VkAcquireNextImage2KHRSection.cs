using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkAcquireNextImage2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkAcquireNextImage2KHR(IntPtr device, VkAcquireNextImageInfoKHR pAcquireInfo, UInt32* pImageIndex);

		public static MgResult AcquireNextImage2KHR(VkDeviceInfo info, MgAcquireNextImageInfoKHR pAcquireInfo, ref UInt32 pImageIndex)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
