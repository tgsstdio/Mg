namespace Magnesium
{
	public class MgBindSparseInfo
	{
		public IMgSemaphore[] WaitSemaphores { get; set; }
		public MgSparseBufferMemoryBindInfo[] BufferBinds { get; set; }
		public MgSparseImageOpaqueMemoryBindInfo[] ImageOpaqueBinds { get; set; }
		public MgSparseImageMemoryBindInfo[] ImageBinds { get; set; }
		public IMgSemaphore[] SignalSemaphores { get; set; }
	}
}

