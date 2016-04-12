namespace Magnesium
{
    public interface IMgQueue
	{
		Result QueueSubmit(MgSubmitInfo[] pSubmits, MgFence fence);
		Result QueueWaitIdle();
		Result QueueBindSparse(MgBindSparseInfo[] pBindInfo, MgFence fence);
		Result QueuePresentKHR(MgPresentInfoKHR pPresentInfo);
	}
}

