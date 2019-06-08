using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetDeviceQueue2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetDeviceQueue2(IntPtr device, [In, Out] VkDeviceQueueInfo2 pQueueInfo, ref IntPtr pQueue);

		public static void GetDeviceQueue2(VkDeviceInfo info, MgDeviceQueueInfo2 pQueueInfo, IMgQueue pQueue)
		{
			// TODO: add implementation
		}
	}
}
