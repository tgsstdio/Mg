using Metal;

namespace Magnesium.Metal
{
	public class AmtDepthStencilStateEncoderState
	{
		public uint BackReference { get; internal set; }
		public IMTLDepthStencilState DepthStencilState { get; internal set; }
		public uint FrontReference { get; internal set; }
	}
}