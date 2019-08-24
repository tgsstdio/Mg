using System;

namespace Magnesium.Vulkan.Functions.Instance
{
    public class VkInstanceInfo
    {
        internal IntPtr Handle { get; set; }
        internal VkInstanceInfo(IntPtr handle)
        {
            Handle = handle;
        }

        internal bool IsDisposed = false;
    }
}