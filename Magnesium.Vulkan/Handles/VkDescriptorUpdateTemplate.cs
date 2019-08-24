using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
    public class VkDescriptorUpdateTemplate : IMgDescriptorUpdateTemplate
    {
        internal UInt64 Handle { get; private set; }
        public VkDescriptorUpdateTemplate(UInt64 handle)
        {
            Handle = handle;
        }

        private bool mIsDisposed = false;
        public void DestroyDescriptorUpdateTemplate(IMgDevice device, IMgAllocationCallbacks allocator)
        {
            if (mIsDisposed)
                return;

            var bDevice = (VkDevice)device;
            Debug.Assert(bDevice != null);

            var bAllocator = (MgVkAllocationCallbacks) allocator;
            IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

            Interops.vkDestroyDescriptorUpdateTemplate(bDevice.Info.Handle, this.Handle, allocatorPtr);

            this.Handle = 0UL;
            mIsDisposed = true;
        }
    }
}
