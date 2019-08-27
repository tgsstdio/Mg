using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkEnumerateDeviceLayerPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkEnumerateDeviceLayerProperties(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkLayerProperties[] pProperties);

        public static MgResult EnumerateDeviceLayerProperties(VkPhysicalDeviceInfo info, out MgLayerProperties[] pProperties)
        {
            uint count = 0U;
            var first = vkEnumerateDeviceLayerProperties(info.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var layers = new VkLayerProperties[count];
            var final = vkEnumerateDeviceLayerProperties(info.Handle, ref count, layers);

            pProperties = new MgLayerProperties[count];
            for (var i = 0; i < count; ++i)
            {
                pProperties[i] = new MgLayerProperties
                {
                    LayerName = VkInteropsUtility.ByteArrayToTrimmedString(layers[i].layerName),
                    SpecVersion = layers[i].specVersion,
                    ImplementationVersion = layers[i].implementationVersion,
                    Description = VkInteropsUtility.ByteArrayToTrimmedString(layers[i].description),
                };
            }

            return final;
        }
    }
}
