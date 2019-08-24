using System;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
    public class VkCommandBufferInfo
    {
        internal IntPtr Handle { get; }
        internal VkCommandBufferInfo(IntPtr handle)
        {
            Handle = handle;
        }
    }
}