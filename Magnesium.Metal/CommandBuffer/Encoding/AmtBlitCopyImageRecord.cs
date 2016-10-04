using Metal;

namespace Magnesium.Metal
{
	public class AmtBlitCopyImageRecord
	{
		public IMTLTexture Source { get; set; }
		public IMTLTexture Destination { get; set; }
		public AmtBlitCopyImageRegionRecord[] Regions { get; set; }
	}
}