using System;

namespace Magnesium.Vulkan
{
    public class VkDebugUtilsMessengerEXT : IMgDebugUtilsMessengerEXT
    {
        private readonly UInt64 mHandle;
        private VkDebugUtilsMessengerData mData;

        public VkDebugUtilsMessengerEXT(
            UInt64 handle,
            VkDebugUtilsMessengerData data
           )
        {
            mHandle = handle;
            mData = data;
        }

        public void DestroyDebugUtilsMessengerEXT(IMgInstance instance, IMgAllocationCallbacks allocator)
        {
            var bInstance = (VkInstance)instance;
            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);

            Interops.vkDestroyDebugUtilsMessengerEXT(bInstance.Info.Handle, this.mHandle, allocatorHandle );

            mData.FreeMemory();
        }
    }    
}
