using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtPipelineEncoderState
	{
		public IMTLRenderPipelineState PipelineState { get; internal set; }
		public MTLCullMode CullMode { get; internal set; }
		public MTLWinding Winding { get; internal set; }
		public MTLTriangleFillMode FillMode { get; internal set; }
		public MTLViewport Viewport { get; internal set; }
		public MTLScissorRect Scissor { get; internal set; }
		public MgColor4f BlendConstants { get; internal set; }
		public float DepthBiasConstantFactor { get; internal set;}
		public float DepthBiasSlopeScale { get; internal set;}
		public float DepthBiasClamp { get; internal set;}
	}
}
