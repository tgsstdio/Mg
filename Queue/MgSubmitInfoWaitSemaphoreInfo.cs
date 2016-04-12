namespace Magnesium
{
    public class MgSubmitInfoWaitSemaphoreInfo
	{
		public MgSemaphore WaitSemaphore { get; set; }
		public MgPipelineStageFlagBits WaitDstStageMask { get; set;}
	}
}

