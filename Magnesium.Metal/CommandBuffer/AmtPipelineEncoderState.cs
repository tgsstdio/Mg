using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtPipelineEncoderState
	{
		public IMTLRenderPipelineState PipelineState { get; internal set; }
		public IMTLDepthStencilState DepthStencilState { get; internal set; }
		public uint FrontReference { get; internal set; }
		public uint BackReference { get; internal set; }
		public MTLCullMode CullMode { get; internal set; }
		public MTLWinding Winding { get; internal set; }
		public MTLTriangleFillMode FillMode { get; internal set; }
		public MTLViewport Viewport { get; internal set; }
		public MTLScissorRect Scissor { get; internal set; }
	}
}
