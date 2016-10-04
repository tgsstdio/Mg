using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtBlitCopyImageRegionRecord
	{
		public MTLSize SourceSize { get; internal set; }

		public nuint SourceSlice { get; internal set; }
		public nuint SourceMipLevel { get; internal set; }
		public uint SourceLayerCount { get; internal set; }
		public MTLOrigin SourceOrigin { get; internal set; }

		public nuint DestinationSlice { get; internal set; }
		public nuint DestinationMipLevel { get; internal set; }
		public uint DestinationLayerCount { get; internal set; }
		public MTLOrigin DestinationOrigin { get; internal set; }
	}
}