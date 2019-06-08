using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueueBeginDebugUtilsLabelEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkQueueBeginDebugUtilsLabelEXT(IntPtr queue, [In, Out] VkDebugUtilsLabelEXT pLabelInfo);

		public static void QueueBeginDebugUtilsLabelEXT(VkQueueInfo info, MgDebugUtilsLabelEXT labelInfo)
		{
			// TODO: add implementation
		}
	}
}
