using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkEnumerateDeviceExtensionPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkEnumerateDeviceExtensionProperties(IntPtr physicalDevice, IntPtr pLayerName, ref UInt32 pPropertyCount, [In, Out] VkExtensionProperties[] pProperties);

        public static MgResult EnumerateDeviceExtensionProperties(VkPhysicalDeviceInfo info, string layerName, out MgExtensionProperties[] pProperties)
        {
            var bLayerName = IntPtr.Zero;

            try
            {
                if (layerName != null)
                {
                    bLayerName = VkInteropsUtility.NativeUtf8FromString(layerName);
                }
                uint count = 0;
                var first = vkEnumerateDeviceExtensionProperties(info.Handle, bLayerName, ref count, null);

                if (first != MgResult.SUCCESS)
                {
                    pProperties = null;
                    return first;
                }

                var extensions = new VkExtensionProperties[count];
                var final = vkEnumerateDeviceExtensionProperties(info.Handle, bLayerName, ref count, extensions);

                pProperties = new MgExtensionProperties[count];
                for (var i = 0; i < count; ++i)
                {
                    pProperties[i] = new MgExtensionProperties
                    {
                        ExtensionName =
                            VkInteropsUtility.ByteArrayToTrimmedString(extensions[i].extensionName),
                        SpecVersion = extensions[i].specVersion,
                    };
                }

                return final;

            }
            finally
            {
                if (bLayerName != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(bLayerName);
                }
            }
        }
    }
}
