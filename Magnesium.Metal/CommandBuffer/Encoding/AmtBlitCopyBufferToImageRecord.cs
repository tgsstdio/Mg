using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtBlitCopyBufferToImageRecord
	{
		public IMTLBuffer Source { get; internal set; }
		public IMTLTexture Destination { get; internal set; }
		public AmtBlitCopyBufferToImageRegionRecord[] Regions { get; internal set; }
	}
}