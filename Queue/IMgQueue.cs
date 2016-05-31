namespace Magnesium
{
    public interface IMgQueue
	{
		Result QueueSubmit(MgSubmitInfo[] pSubmits, IMgFence fence);
		Result QueueWaitIdle();
		Result QueueBindSparse(MgBindSparseInfo[] pBindInfo, IMgFence fence);
		Result QueuePresentKHR(MgPresentInfoKHR pPresentInfo);
	}
}

