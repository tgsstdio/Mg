using System;

namespace Magnesium
{
	public class MgBindBufferMemoryInfo
	{
		public IMgBuffer Buffer { get; set; }
		public IMgDeviceMemory Memory { get; set; }
		public UInt64 MemoryOffset { get; set; }
	}
}
