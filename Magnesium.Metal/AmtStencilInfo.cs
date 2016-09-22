using Metal;

namespace Magnesium.Metal
{
	public class AmtStencilInfo
	{
		public MTLCompareFunction StencilCompareFunction { get; internal set; }
		public MTLStencilOperation DepthFailure { get; internal set; }
		public MTLStencilOperation DepthStencilPass { get; internal set; }
		public uint ReadMask { get; internal set; }
		public MTLStencilOperation StencilFailure { get; internal set; }
		public uint WriteMask { get; internal set; }
	}
}