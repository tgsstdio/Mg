using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetFenceStatusSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetFenceStatus(IntPtr device, UInt64 fence);

        public static MgResult GetFenceStatus(VkDeviceInfo info, IMgFence fence)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bFence = (VkFence)fence;
            Debug.Assert(bFence != null);

            return vkGetFenceStatus(info.Handle, bFence.Handle);
        }
    }
}
