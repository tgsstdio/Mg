using System;

namespace Magnesium.Vulkan.Functions.Queue
{
    public class VkQueueInfo
    {
        internal IntPtr Handle = IntPtr.Zero;
        public VkQueueInfo(IntPtr handle)
        {
            Handle = handle;
        }

        internal bool IsDisposed = false;
    }
}