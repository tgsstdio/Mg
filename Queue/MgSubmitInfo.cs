namespace Magnesium
{
    public class MgSubmitInfo
	{
		public MgSubmitInfoWaitSemaphoreInfo[] WaitSemaphores { get; set; }
		public IMgCommandBuffer[] CommandBuffers { get; set; }
		public IMgSemaphore[] SignalSemaphores { get; set; }
	}
}

