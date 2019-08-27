using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkBindImageMemory2Section
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkBindImageMemory2(IntPtr device, UInt32 bindInfoCount, [In] VkBindImageMemoryInfo[] pBindInfos);

        public static MgResult BindImageMemory2(VkDeviceInfo info, MgBindImageMemoryInfo[] pBindInfos)
        {
            var bindInfoCount = (UInt32)pBindInfos.Length;

            var bBindInfos = new VkBindImageMemoryInfo[bindInfoCount];

            for (var i = 0; i < bindInfoCount; i += 1)
            {
                var currentInfo = pBindInfos[i];

                var bImage = (VkImage)currentInfo.Image;
                var bImagePtr = bImage != null ? bImage.Handle : 0UL;

                var bDeviceMemory = (VkDeviceMemory)currentInfo.Memory;
                var bDeviceMemoryPtr = bDeviceMemory != null ? bDeviceMemory.Handle : 0UL;

                bBindInfos[i] = new VkBindImageMemoryInfo
                {
                    sType = VkStructureType.StructureTypeBindImageMemoryInfo,
                    // TODO: extensible
                    pNext = IntPtr.Zero,
                    image = bImagePtr,
                    memory = bDeviceMemoryPtr,
                    memoryOffset = currentInfo.MemoryOffset,
                };
            }

            return vkBindImageMemory2(info.Handle, bindInfoCount, bBindInfos);
        }
    }
}
