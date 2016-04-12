using System;

namespace Magnesium
{
    public class MgSparseImageMemoryBindInfo
	{
		public MgImage Image { get; set; }
		public UInt32 BindCount { get; set; }
		public MgSparseImageMemoryBind[] Binds { get; set; }
	}
}

