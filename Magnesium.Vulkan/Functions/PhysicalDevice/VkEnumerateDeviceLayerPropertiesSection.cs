using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkEnumerateDeviceLayerPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkEnumerateDeviceLayerProperties(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkLayerProperties[] pProperties);

		public static MgResult EnumerateDeviceLayerProperties(VkPhysicalDeviceInfo info, out MgLayerProperties[] pProperties)
		{
			// TODO: add implementation
		}
	}
}
