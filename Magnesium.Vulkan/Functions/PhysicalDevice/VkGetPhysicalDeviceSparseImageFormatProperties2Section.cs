using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceSparseImageFormatProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceSparseImageFormatProperties2(IntPtr physicalDevice, ref VkPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, ref UInt32 pPropertyCount, [In, Out] VkSparseImageFormatProperties2[] pProperties);

        public static void GetPhysicalDeviceSparseImageFormatProperties2(VkPhysicalDeviceInfo info, MgPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, out MgSparseImageFormatProperties2[] pProperties)
        {
            if (pFormatInfo == null)
                throw new ArgumentNullException(nameof(pFormatInfo));

            uint count = 0;

            var bFormatInfo = new VkPhysicalDeviceSparseImageFormatInfo2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceSparseImageFormatInfo2,
                pNext = IntPtr.Zero, // TODO: extension
                format = pFormatInfo.Format,
                samples = pFormatInfo.Samples,
                tiling = pFormatInfo.Tiling,
                type = (VkImageType)pFormatInfo.Type,
                usage = pFormatInfo.Usage,
            };

            vkGetPhysicalDeviceSparseImageFormatProperties2
            (
                info.Handle,
                ref bFormatInfo,
                ref count,
                null
            );


            pProperties = new MgSparseImageFormatProperties2[count];

            if (count > 0)
            {

                var bFormatProperties = new VkSparseImageFormatProperties2[count];
                for (var i = 0; i < count; i += 1)
                {
                    bFormatProperties[i] = new VkSparseImageFormatProperties2
                    {
                        sType = VkStructureType.StructureTypeSparseImageFormatProperties2,
                        pNext = IntPtr.Zero, // TODO: extension
                    };
                }

                vkGetPhysicalDeviceSparseImageFormatProperties2
                (
                    info.Handle,
                    ref bFormatInfo,
                    ref count,
                    bFormatProperties
                );

                for (var i = 0; i < count; i += 1)
                {
                    pProperties[i] = new MgSparseImageFormatProperties2
                    {
                        Properties = VkGetPhysicalDeviceSparseImageFormatPropertiesSection.TranslateSparseImageFormatProperties(ref bFormatProperties[i].properties),
                    };
                }
            }

        }
    }
}
