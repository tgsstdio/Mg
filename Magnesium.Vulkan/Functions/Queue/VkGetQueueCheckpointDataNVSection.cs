using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkGetQueueCheckpointDataNVSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetQueueCheckpointDataNV(IntPtr queue, UInt32* pCheckpointDataCount, VkCheckpointDataNV* pCheckpointData);

		public static void GetQueueCheckpointDataNV(VkQueueInfo info, out MgCheckpointDataNV[] pCheckpointData)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
