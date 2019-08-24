using System;

namespace Magnesium
{
	public class MgConditionalRenderingBeginInfoEXT
	{
		public IMgBuffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt32 Flags { get; set; }
	}
}
