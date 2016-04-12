using System;

namespace Magnesium
{
    public class MgImageFormatProperties
	{
		public MgExtent3D MaxExtent { get; set; }
		public UInt32 MaxMipLevels { get; set; }
		public UInt32 MaxArrayLayers { get; set; }
		public MgSampleCountFlagBits SampleCounts { get; set; }
		public UInt64 MaxResourceSize { get; set; }
	}
}

