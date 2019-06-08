using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueueEndDebugUtilsLabelEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkQueueEndDebugUtilsLabelEXT(IntPtr queue);

		public static void QueueEndDebugUtilsLabelEXT(VkQueueInfo info)
		{
			// TODO: add implementation
		}
	}
}
