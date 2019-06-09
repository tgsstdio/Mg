using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateBufferViewSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateBufferView(IntPtr device, ref VkBufferViewCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pView);

        public static MgResult CreateBufferView(VkDeviceInfo info, MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var bBuffer = (VkBuffer)pCreateInfo.Buffer;
            Debug.Assert(bBuffer != null);

            var createInfo = new VkBufferViewCreateInfo
            {
                sType = VkStructureType.StructureTypeBufferViewCreateInfo,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
                buffer = bBuffer.Handle,
                format = pCreateInfo.Format,
                offset = pCreateInfo.Offset,
                range = pCreateInfo.Range,
            };
            var internalHandle = 0UL;
            var result = vkCreateBufferView(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
            pView = new VkBufferView(internalHandle);
            return result;
        }
    }
}
