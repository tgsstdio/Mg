using System;
using Magnesium.Vulkan.Functions.Queue;

namespace Magnesium.Vulkan
{
    public class VkQueue : IMgQueue
	{
		public VkQueueInfo Info { get; }
		internal VkQueue(IntPtr handle)
		{
            Info = new VkQueueInfo(handle);
		}

		public MgResult QueueSubmit(MgSubmitInfo[] pSubmits, IMgFence fence)
        {
            return VkQueueSubmitSection.QueueSubmit(Info, pSubmits, fence);
        }

        public MgResult QueueWaitIdle()
        {
            return VkQueueWaitIdleSection.QueueWaitIdle(Info);
        }

        public MgResult QueueBindSparse(MgBindSparseInfo[] pBindInfo, IMgFence fence)
        {
            return VkQueueBindSparseSection.QueueBindSparse(Info, pBindInfo, fence);
        }

        public MgResult QueuePresentKHR(MgPresentInfoKHR pPresentInfo)
        {
            return VkQueuePresentKHRSection.QueuePresentKHR(Info, pPresentInfo);
        }

        public void GetQueueCheckpointDataNV(out MgCheckpointDataNV[] pCheckpointData)
        {
            VkGetQueueCheckpointDataNVSection.GetQueueCheckpointDataNV(Info, out pCheckpointData);
        }

        public void QueueBeginDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo)
        {
            VkQueueBeginDebugUtilsLabelEXTSection.QueueBeginDebugUtilsLabelEXT(Info, labelInfo);
        }

        public void QueueInsertDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo)
        {
            VkQueueInsertDebugUtilsLabelEXTSection.QueueInsertDebugUtilsLabelEXT(Info, labelInfo);
        }

        public void QueueEndDebugUtilsLabelEXT()
        {
            VkQueueEndDebugUtilsLabelEXTSection.QueueEndDebugUtilsLabelEXT(Info);
        }
    }
}
