using Metal;

namespace Magnesium.Metal
{
	class AmtDepthStencilStateEncoderState
	{
		public uint BackReference { get; internal set; }
		public IMTLDepthStencilState DepthStencilState { get; internal set; }
		public uint FrontReference { get; internal set; }
	}
}