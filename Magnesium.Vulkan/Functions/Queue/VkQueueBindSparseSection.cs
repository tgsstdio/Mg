using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueueBindSparseSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkQueueBindSparse(IntPtr queue, UInt32 bindInfoCount, [In, Out] VkBindSparseInfo[] pBindInfo, UInt64 fence);

		public static MgResult QueueBindSparse(VkQueueInfo info, MgBindSparseInfo[] pBindInfo, IMgFence fence)
		{
			// TODO: add implementation
		}
	}
}
