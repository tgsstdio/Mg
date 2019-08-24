using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceExternalBufferPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceExternalBufferProperties(IntPtr physicalDevice, ref VkPhysicalDeviceExternalBufferInfo pExternalBufferInfo, ref VkExternalBufferProperties pExternalBufferProperties);

        public static void GetPhysicalDeviceExternalBufferProperties(VkPhysicalDeviceInfo info, MgPhysicalDeviceExternalBufferInfo pExternalBufferInfo, out MgExternalBufferProperties pExternalBufferProperties)
        {
            var bExternalBufferInfo = new VkPhysicalDeviceExternalBufferInfo
            {
                sType = VkStructureType.StructureTypePhysicalDeviceExternalBufferInfo,
                // TODO: extension
                pNext = IntPtr.Zero,
                flags = (VkBufferCreateFlags)pExternalBufferInfo.Flags,
                handleType = pExternalBufferInfo.HandleType,
                usage = pExternalBufferInfo.Usage,
            };

            var output = new VkExternalBufferProperties
            {
                sType = VkStructureType.StructureTypeExternalBufferProperties,
                pNext = IntPtr.Zero,
            };

            vkGetPhysicalDeviceExternalBufferProperties(info.Handle, ref bExternalBufferInfo, ref output);

            pExternalBufferProperties = new MgExternalBufferProperties
            {
                ExternalMemoryProperties = new MgExternalMemoryProperties
                {
                    CompatibleHandleTypes = output.externalMemoryProperties.compatibleHandleTypes,
                    ExportFromImportedHandleTypes = output.externalMemoryProperties.exportFromImportedHandleTypes,
                    ExternalMemoryFeatures = output.externalMemoryProperties.externalMemoryFeatures,
                },
            };

        }
    }
}
