using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtBlitCopyBufferToImageRecord
	{
		public IMTLBuffer Buffer { get; internal set; }
		public IMTLTexture Image { get; internal set; }
		public AmtBlitCopyBufferToImageRegionRecord[] Regions { get; internal set; }
	}
}