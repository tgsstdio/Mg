using System;

namespace Magnesium
{
    public class MgSparseMemoryBind
	{
		public UInt64 ResourceOffset { get; set; }
		public UInt64 Size { get; set; }
		public MgDeviceMemory Memory { get; set; }
		public UInt64 MemoryOffset { get; set; }
		public MgSparseMemoryBindFlagBits Flags { get; set; }
	}
}

