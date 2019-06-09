using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueueWaitIdleSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkQueueWaitIdle(IntPtr queue);

		public static MgResult QueueWaitIdle(VkQueueInfo info)
		{
			// TODO: add implementation
			throw new NotImplementedException();
		}
	}
}
