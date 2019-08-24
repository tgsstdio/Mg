using System;

namespace Magnesium.Vulkan.Functions.Device
{
    public class VkDeviceInfo
    {
        internal IntPtr Handle = IntPtr.Zero;
        public VkDeviceInfo(IntPtr handle)
        {
            Handle = handle;
        }

        internal bool IsDisposed = false;
    }
}
