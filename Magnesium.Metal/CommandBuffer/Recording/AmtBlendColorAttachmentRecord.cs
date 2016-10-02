using Metal;

namespace Magnesium.Metal
{
	public class AmtBlendColorAttachmentRecord
	{
		public bool IsBlendingEnabled { get; internal set; }
		public MTLBlendOperation RgbBlendOperation { get; internal set;}
		public MTLBlendOperation AlphaBlendOperation { get; internal set; }
		public MTLBlendFactor SourceRgbBlendFactor { get; internal set;}
		public MTLBlendFactor DestinationRgbBlendFactor { get; internal set;}
		public MTLBlendFactor SourceAlphaBlendFactor { get; internal set;}
		public MTLBlendFactor DestinationAlphaBlendFactor { get; internal set;}
		public MTLColorWriteMask ColorWriteMask { get; internal set; }
	}
}