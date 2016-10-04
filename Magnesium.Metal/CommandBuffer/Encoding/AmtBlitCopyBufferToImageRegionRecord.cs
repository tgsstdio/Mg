using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtBlitCopyBufferToImageRegionRecord
	{
		public nuint SourceSizePerImage { get; internal set; }

		public nuint DestinationSlice { get; internal set; }
		public uint LayerCount { get; internal set; }

		public MTLOrigin DestinationOffset { get; internal set; }

		public nuint DestinationLevel { get; internal set; }

		public MTLSize SourceSize { get; internal set; }

		public nuint SourceBytesPerRow { get; internal set; }
		public nuint SourceOffset { get; internal set; }
	}
}