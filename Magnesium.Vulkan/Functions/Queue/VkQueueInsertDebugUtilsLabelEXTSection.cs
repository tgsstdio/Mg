using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueueInsertDebugUtilsLabelEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkQueueInsertDebugUtilsLabelEXT(IntPtr queue, [In, Out] VkDebugUtilsLabelEXT pLabelInfo);

		public static void QueueInsertDebugUtilsLabelEXT(VkQueueInfo info, MgDebugUtilsLabelEXT labelInfo)
		{
			// TODO: add implementation
		}
	}
}
