using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsEncoderItemGrid
	{
		public MTLRenderPassDescriptor[] RenderPasses { get; internal set;}
		public MgColor4f[] BlendConstants { get; internal set; }
		public AmtPipelineEncoderState[] Pipelines { get; internal set; }
		public IMTLDepthStencilState[] DepthStencilStates { get; internal set; }
		public AmtDepthBiasEncoderState[] DepthBias { get; internal set; }
		public AmtStencilReferenceEncoderState[] StencilReferences { get; internal set; }
		public MTLViewport[] Viewports { get; internal set; }
	}
}
