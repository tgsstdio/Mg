using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSparseImageFormatProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceSparseImageFormatProperties2(IntPtr physicalDevice, [In, Out] VkPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, ref UInt32 pPropertyCount, [In, Out] VkSparseImageFormatProperties2[] pProperties);

		public static void GetPhysicalDeviceSparseImageFormatProperties2(VkPhysicalDeviceInfo info, MgPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, out MgSparseImageFormatProperties2[] pProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
