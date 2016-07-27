using System;

namespace Magnesium
{
    // Device
    public class MgDescriptorBufferInfo
	{
		public IMgBuffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Range { get; set; }
	}

}

