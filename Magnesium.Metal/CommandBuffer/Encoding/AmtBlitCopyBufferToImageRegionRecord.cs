using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtBlitCopyBufferToImageRegionRecord
	{
		public nuint BufferImageAllocationSize { get; internal set; }

		public nuint BaseArrayLayer { get; internal set; }
		public uint ImageLayerCount { get; internal set; }

		public MTLOrigin ImageOffset { get; internal set; }

		public nuint ImageMipLevel { get; internal set; }

		public MTLSize ImageSize { get; internal set; }

		public nuint BufferBytesPerRow { get; internal set; }
		public nuint BufferOffset { get; internal set; }
	}
}