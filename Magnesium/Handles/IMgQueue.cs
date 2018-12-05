namespace Magnesium
{
    public interface IMgQueue
	{
		MgResult QueueSubmit(MgSubmitInfo[] pSubmits, IMgFence fence);
		MgResult QueueWaitIdle();
		MgResult QueueBindSparse(MgBindSparseInfo[] pBindInfo, IMgFence fence);
		MgResult QueuePresentKHR(MgPresentInfoKHR pPresentInfo);
	}
}

