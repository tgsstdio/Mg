using System;

namespace Magnesium
{
    public class MgBindSparseInfo
	{
		public UInt32 WaitSemaphoreCount { get; set; }
		public MgSemaphore[] WaitSemaphores { get; set; }
		public UInt32 BufferBindCount { get; set; }
		public MgSparseBufferMemoryBindInfo[] BufferBinds { get; set; }
		public UInt32 ImageOpaqueBindCount { get; set; }
		public MgSparseImageOpaqueMemoryBindInfo[] ImageOpaqueBinds { get; set; }
		public UInt32 ImageBindCount { get; set; }
		public MgSparseImageMemoryBindInfo[] ImageBinds { get; set; }
		public UInt32 SignalSemaphoreCount { get; set; }
		public MgSemaphore[] SignalSemaphores { get; set; }
	}
}

