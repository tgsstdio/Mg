using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceExternalSemaphorePropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceExternalSemaphoreProperties(IntPtr physicalDevice, ref VkPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, ref VkExternalSemaphoreProperties pExternalSemaphoreProperties);

        public static void GetPhysicalDeviceExternalSemaphoreProperties(VkPhysicalDeviceInfo info, MgPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, out MgExternalSemaphoreProperties pExternalSemaphoreProperties)
        {
            var bExternalSemaphoreInfo = new VkPhysicalDeviceExternalSemaphoreInfo
            {
                sType = VkStructureType.StructureTypePhysicalDeviceExternalSemaphoreInfo,
                pNext = IntPtr.Zero, // TODO: extension
                handleType = pExternalSemaphoreInfo.HandleType,
            };

            var output = new VkExternalSemaphoreProperties
            {
                sType = VkStructureType.StructureTypeExternalSemaphoreProperties,
                pNext = IntPtr.Zero,
            };

            vkGetPhysicalDeviceExternalSemaphoreProperties(info.Handle,
                ref bExternalSemaphoreInfo,
                ref output);

            pExternalSemaphoreProperties = new MgExternalSemaphoreProperties
            {
                CompatibleHandleTypes = output.compatibleHandleTypes,
                ExportFromImportedHandleTypes = output.exportFromImportedHandleTypes,
                ExternalSemaphoreFeatures = output.externalSemaphoreFeatures,
            };
        }
    }
}
