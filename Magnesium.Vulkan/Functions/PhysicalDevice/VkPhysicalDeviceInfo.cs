using System;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
    public class VkPhysicalDeviceInfo
    {
        public IntPtr Handle { get; }
        public VkPhysicalDeviceInfo(IntPtr handle)
        {
            Handle = handle;
        }
    }
}