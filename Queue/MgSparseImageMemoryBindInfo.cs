using System;

namespace Magnesium
{
    public class MgSparseImageMemoryBindInfo
	{
		public IMgImage Image { get; set; }
		public UInt32 BindCount { get; set; }
		public MgSparseImageMemoryBind[] Binds { get; set; }
	}
}

