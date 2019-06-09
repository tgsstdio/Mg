using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkBindBufferMemory2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkBindBufferMemory2(IntPtr device, UInt32 bindInfoCount, [In] VkBindBufferMemoryInfo[] pBindInfos);

        public static MgResult BindBufferMemory2(VkDeviceInfo info, MgBindBufferMemoryInfo[] pBindInfos)
        {
            var bindInfoCount = (UInt32)pBindInfos.Length;

            var bBindInfos = new VkBindBufferMemoryInfo[bindInfoCount];

            for (var i = 0; i < bindInfoCount; i += 1)
            {
                var currentInfo = pBindInfos[i];

                var bBuffer = (VkBuffer)currentInfo.Memory;
                var bBufferPtr = bBuffer != null ? bBuffer.Handle : 0UL;

                var bDeviceMemory = (VkDeviceMemory)currentInfo.Memory;
                var bDeviceMemoryPtr = bDeviceMemory != null ? bDeviceMemory.Handle : 0UL;

                bBindInfos[i] = new VkBindBufferMemoryInfo
                {
                    sType = VkStructureType.StructureTypeBindBufferMemoryInfo,
                    // TODO: extensible
                    pNext = IntPtr.Zero,
                    buffer = bBufferPtr,
                    memory = bDeviceMemoryPtr,
                    memoryOffset = currentInfo.MemoryOffset,
                };
            }

            return vkBindBufferMemory2(info.Handle, bindInfoCount, bBindInfos);
        }
    }
}
