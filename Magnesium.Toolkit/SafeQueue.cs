using System;
namespace Magnesium.Toolkit
{
	public class SafeQueue : IMgQueue
	{
		internal IMgQueue mImpl = null;
		internal SafeQueue(IMgQueue impl)
		{
			mImpl = impl;
		}

		public MgResult QueueSubmit(MgSubmitInfo[] pSubmits, IMgFence fence) {
			Validation.Queue.QueueSubmit.Validate(pSubmits, fence);
			return mImpl.QueueSubmit(pSubmits, fence);
		}

		public MgResult QueueWaitIdle() {
			return mImpl.QueueWaitIdle();
		}

		public MgResult QueueBindSparse(MgBindSparseInfo[] pBindInfo, IMgFence fence) {
			Validation.Queue.QueueBindSparse.Validate(pBindInfo, fence);
			return mImpl.QueueBindSparse(pBindInfo, fence);
		}

		public MgResult QueuePresentKHR(MgPresentInfoKHR pPresentInfo) {
			Validation.Queue.QueuePresentKHR.Validate(pPresentInfo);
			return mImpl.QueuePresentKHR(pPresentInfo);
		}

		public void GetQueueCheckpointDataNV(out MgCheckpointDataNV[] pCheckpointData) {
			mImpl.GetQueueCheckpointDataNV(out pCheckpointData);
		}

		public void QueueBeginDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo) {
			Validation.Queue.QueueBeginDebugUtilsLabelEXT.Validate(labelInfo);
			mImpl.QueueBeginDebugUtilsLabelEXT(labelInfo);
		}

		public void QueueInsertDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo) {
			Validation.Queue.QueueInsertDebugUtilsLabelEXT.Validate(labelInfo);
			mImpl.QueueInsertDebugUtilsLabelEXT(labelInfo);
		}

		public void QueueEndDebugUtilsLabelEXT() {
			mImpl.QueueEndDebugUtilsLabelEXT();
		}

	}
}
