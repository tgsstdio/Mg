using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueueSubmitSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkQueueSubmit(IntPtr queue, UInt32 submitCount, [In, Out] VkSubmitInfo[] pSubmits, UInt64 fence);

		public static MgResult QueueSubmit(VkQueueInfo info, MgSubmitInfo[] pSubmits, IMgFence fence)
		{
			// TODO: add implementation
		}
	}
}
