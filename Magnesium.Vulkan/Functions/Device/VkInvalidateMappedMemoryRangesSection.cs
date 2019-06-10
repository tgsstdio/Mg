using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkInvalidateMappedMemoryRangesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe MgResult vkInvalidateMappedMemoryRanges(IntPtr device, UInt32 memoryRangeCount, VkMappedMemoryRange* pMemoryRanges);

        public static MgResult InvalidateMappedMemoryRanges(VkDeviceInfo info, MgMappedMemoryRange[] pMemoryRanges)
		{
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            unsafe
            {
                var rangeCount = (uint)pMemoryRanges.Length;

                var ranges = stackalloc VkMappedMemoryRange[pMemoryRanges.Length];

                for (var i = 0; i < rangeCount; ++i)
                {
                    var current = pMemoryRanges[i];
                    var bDeviceMemory = (VkDeviceMemory)current.Memory;
                    Debug.Assert(bDeviceMemory != null);

                    ranges[i] = new VkMappedMemoryRange
                    {
                        sType = VkStructureType.StructureTypeMappedMemoryRange,
                        pNext = IntPtr.Zero,
                        memory = bDeviceMemory.Handle,
                        offset = current.Offset,
                        size = current.Size
                    };
                }

                return vkInvalidateMappedMemoryRanges(info.Handle, rangeCount, ranges);
            }
        }
	}
}
