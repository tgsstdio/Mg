using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceExternalFencePropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceExternalFenceProperties(IntPtr physicalDevice, ref VkPhysicalDeviceExternalFenceInfo pExternalFenceInfo, ref VkExternalFenceProperties pExternalFenceProperties);

        public static void GetPhysicalDeviceExternalFenceProperties(VkPhysicalDeviceInfo info, MgPhysicalDeviceExternalFenceInfo pExternalFenceInfo, out MgExternalFenceProperties pExternalFenceProperties)
        {
            var bExternalFenceInfo = new VkPhysicalDeviceExternalFenceInfo
            {
                sType = VkStructureType.StructureTypePhysicalDeviceExternalFenceInfo,
                // TODO: extension
                pNext = IntPtr.Zero,
                handleType = pExternalFenceInfo.HandleType,
            };

            var output = new VkExternalFenceProperties
            {
                sType = VkStructureType.StructureTypeExternalFenceProperties,
                pNext = IntPtr.Zero,
            };

            vkGetPhysicalDeviceExternalFenceProperties(info.Handle, ref bExternalFenceInfo, ref output);

            pExternalFenceProperties = new MgExternalFenceProperties
            {
                CompatibleHandleTypes = output.compatibleHandleTypes,
                ExternalFenceFeatures = output.externalFenceFeatures,
                ExportFromImportedHandleTypes = output.exportFromImportedHandleTypes,
            };
        }
    }
}
