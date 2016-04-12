using System;

namespace Magnesium
{
    public class MgSubresourceLayout
	{
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
		public UInt64 RowPitch { get; set; }
		public UInt64 ArrayPitch { get; set; }
		public UInt64 DepthPitch { get; set; }
	}
}

