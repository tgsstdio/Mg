using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkDeviceWaitIdleSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkDeviceWaitIdle(IntPtr device);

        public static MgResult DeviceWaitIdle(VkDeviceInfo info)
		{
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            return vkDeviceWaitIdle(info.Handle);
        }
	}
}
