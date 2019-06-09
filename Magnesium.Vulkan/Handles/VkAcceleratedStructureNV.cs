using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
    public class VkAccelerationStructureNV : IMgAccelerationStructureNV
    {
        internal UInt64 Handle { get; private set; }
        internal VkAccelerationStructureNV(UInt64 handle)
        {
            Handle = handle;
        }

        private bool mIsDisposed = false;
        public void DestroyAccelerationStructureNV(IMgDevice device, IMgAllocationCallbacks allocator)
        {
            if (mIsDisposed)
                return;

            var bDevice = (VkDevice)device;
            Debug.Assert(bDevice != null);

            var bAllocator = (MgVkAllocationCallbacks)allocator;
            IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

            Interops.vkDestroyAccelerationStructureNV(bDevice.Info.Handle, this.Handle, allocatorPtr);

            this.Handle = 0UL;
            mIsDisposed = true;
        }
    }
}
