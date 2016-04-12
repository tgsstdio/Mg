using System;

namespace Magnesium
{
    public class MgSparseImageMemoryBind
	{
		public MgImageSubresource Subresource { get; set; }
		public MgOffset3D Offset { get; set; }
		public MgExtent3D Extent { get; set; }
		public MgDeviceMemory Memory { get; set; }
		public UInt64 MemoryOffset { get; set; }
		public MgSparseMemoryBindFlagBits Flags { get; set; }
	}
}

