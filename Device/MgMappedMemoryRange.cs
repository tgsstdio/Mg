using System;

namespace Magnesium
{
    public class MgMappedMemoryRange
	{
		public MgDeviceMemory Memory { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
	}
}

