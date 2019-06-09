using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkEnumerateDeviceExtensionPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkEnumerateDeviceExtensionProperties(IntPtr physicalDevice, string pLayerName, ref UInt32 pPropertyCount, [In, Out] VkExtensionProperties[] pProperties);

		public static MgResult EnumerateDeviceExtensionProperties(VkPhysicalDeviceInfo info, string layerName, out MgExtensionProperties[] pProperties)
		{
			// TODO: add implementation
		}
	}
}
