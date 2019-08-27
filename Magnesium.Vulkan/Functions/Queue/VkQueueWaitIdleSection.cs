using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public static class VkQueueWaitIdleSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkQueueWaitIdle(IntPtr queue);

        public static MgResult QueueWaitIdle(VkQueueInfo info)
        {
            return vkQueueWaitIdle(info.Handle);
        }
    }
}
