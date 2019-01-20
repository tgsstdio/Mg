namespace Magnesium
{
    public interface IMgQueue
	{
		MgResult QueueSubmit(MgSubmitInfo[] pSubmits, IMgFence fence);
		MgResult QueueWaitIdle();
		MgResult QueueBindSparse(MgBindSparseInfo[] pBindInfo, IMgFence fence);
		MgResult QueuePresentKHR(MgPresentInfoKHR pPresentInfo);

        void GetQueueCheckpointDataNV(out MgCheckpointDataNV[] pCheckpointData);
        void QueueBeginDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo);
        void QueueInsertDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo);
        void QueueEndDebugUtilsLabelEXT();

        //MgResult QueueSignalReleaseImageANDROID(
        //    IMgSemaphore[] waitSemaphores,
        //    IMgImage image,
        //    ref int pNativeFenceFd);
    }
}

