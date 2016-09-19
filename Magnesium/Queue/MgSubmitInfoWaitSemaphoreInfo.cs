namespace Magnesium
{
    public class MgSubmitInfoWaitSemaphoreInfo
	{
		public IMgSemaphore WaitSemaphore { get; set; }
		public MgPipelineStageFlagBits WaitDstStageMask { get; set;}
	}
}

