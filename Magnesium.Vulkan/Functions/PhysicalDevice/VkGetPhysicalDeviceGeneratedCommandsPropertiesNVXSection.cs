using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceGeneratedCommandsPropertiesNVXSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX(IntPtr physicalDevice, ref VkDeviceGeneratedCommandsFeaturesNVX pFeatures, ref VkDeviceGeneratedCommandsLimitsNVX pLimits);

        public static void GetPhysicalDeviceGeneratedCommandsPropertiesNVX(VkPhysicalDeviceInfo info, MgDeviceGeneratedCommandsFeaturesNVX pFeatures, out MgDeviceGeneratedCommandsLimitsNVX pLimits)
        {
            var bFeatures = new VkDeviceGeneratedCommandsFeaturesNVX
            {
                sType = VkStructureType.StructureTypeDeviceGeneratedCommandsFeaturesNvx,
                pNext = IntPtr.Zero, // TODO: extension
                computeBindingPointSupport = VkBool32.ConvertTo(pFeatures.ComputeBindingPointSupport),
            };

            var output = new VkDeviceGeneratedCommandsLimitsNVX
            {
                sType = VkStructureType.StructureTypeDeviceGeneratedCommandsLimitsNvx,
                pNext = IntPtr.Zero, // TODO : extension
            };

            vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX(
                info.Handle,
                ref bFeatures,
                ref output);

            pLimits = new MgDeviceGeneratedCommandsLimitsNVX
            {
                MaxIndirectCommandsLayoutTokenCount = output.maxIndirectCommandsLayoutTokenCount,
                MaxObjectEntryCounts = output.maxObjectEntryCounts,
                MinCommandsTokenBufferOffsetAlignment = output.minCommandsTokenBufferOffsetAlignment,
                MinSequenceCountBufferOffsetAlignment = output.minSequenceCountBufferOffsetAlignment,
                MinSequenceIndexBufferOffsetAlignment = output.minSequenceIndexBufferOffsetAlignment,
            };
        }
    }
}
