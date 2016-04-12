using System;

namespace Magnesium
{
    public class MgStencilOpState
	{
		public MgStencilOp FailOp { get; set; }
		public MgStencilOp PassOp { get; set; }
		public MgStencilOp DepthFailOp { get; set; }
		public MgCompareOp CompareOp { get; set; }
		public UInt32 CompareMask { get; set; }
		public UInt32 WriteMask { get; set; }
		public UInt32 Reference { get; set; }
	}
}

