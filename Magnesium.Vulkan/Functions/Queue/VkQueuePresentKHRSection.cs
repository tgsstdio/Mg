using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueuePresentKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkQueuePresentKHR(IntPtr queue, VkPresentInfoKHR pPresentInfo);

		public static MgResult QueuePresentKHR(VkQueueInfo info, MgPresentInfoKHR pPresentInfo)
		{
			// TODO: add implementation
		}
	}
}
