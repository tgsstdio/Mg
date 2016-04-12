namespace Magnesium
{
    public class MgSubmitInfo
	{
		public MgSubmitInfoWaitSemaphoreInfo[] WaitSemaphores { get; set; }
		public IMgCommandBuffer[] CommandBuffers { get; set; }
		public MgSemaphore[] SignalSemaphores { get; set; }
	}
}

