using System;

namespace Magnesium
{
    public class MgBufferViewCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgBuffer Buffer { get; set; }
		public MgFormat Format { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Range { get; set; }
	}
}

