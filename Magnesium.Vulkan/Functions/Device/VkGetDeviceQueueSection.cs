using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetDeviceQueueSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetDeviceQueue(IntPtr device, UInt32 queueFamilyIndex, UInt32 queueIndex, ref IntPtr pQueue);

		public static void GetDeviceQueue(VkDeviceInfo info, UInt32 queueFamilyIndex, UInt32 queueIndex, out IMgQueue pQueue)
		{
			// TODO: add implementation
		}
	}
}
