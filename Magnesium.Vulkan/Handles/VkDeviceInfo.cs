using System;

namespace Magnesium.Vulkan
{
    public class VkDeviceInfo
    {
        internal IntPtr Handle = IntPtr.Zero;
        public VkDeviceInfo(IntPtr handle)
        {
            Handle = handle;
        }

        internal bool IsDisposed = false;

        /// <summary>
        /// Allocator is optional
        /// </summary>
        /// <param name="allocator"></param>
        /// <returns></returns>
        public static IntPtr GetAllocatorHandle(IMgAllocationCallbacks allocator)
        {
            var bAllocator = (MgVkAllocationCallbacks)allocator;
            return bAllocator != null ? bAllocator.Handle : IntPtr.Zero;
        }
    }
}
