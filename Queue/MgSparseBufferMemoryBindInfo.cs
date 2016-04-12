using System;

namespace Magnesium
{
    public class MgSparseBufferMemoryBindInfo
	{
		public MgBuffer Buffer { get; set; }
		public UInt32 BindCount { get; set; }
		public MgSparseMemoryBind[] Binds { get; set; }
	}
}

