using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateEventSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateEvent(IntPtr device, ref VkEventCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pEvent);

        public static MgResult CreateEvent(VkDeviceInfo info, MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var createInfo = new VkEventCreateInfo
            {
                sType = VkStructureType.StructureTypeEventCreateInfo,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
            };

            var eventHandle = 0UL;
            var result = vkCreateEvent(info.Handle, ref createInfo, allocatorPtr, ref eventHandle);
            @event = new VkEvent(eventHandle);

            return result;
        }
    }
}
