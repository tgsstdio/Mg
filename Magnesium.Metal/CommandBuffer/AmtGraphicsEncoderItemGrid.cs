using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsEncoderItemGrid
	{
		public MTLRenderPassDescriptor[] RenderPasses { get; internal set;}
		public MgColor4f[] BlendConstants { get; internal set; }
		public AmtPipelineEncoderState[] Pipelines { get; internal set; }
		public AmtDepthStencilStateEncoderState[] DepthStencilStates { get; internal set; }
		public AmtDepthBiasEncoderState[] DepthBias { get; internal set; }
		public AmtStencilReferenceEncoderState[] StencilReferences { get; internal set; }
		public MTLScissorRect[] Scissors { get; internal set;}
		public MTLViewport[] Viewports { get; internal set; }
		public AmtDrawEncoderState[] Draws { get; internal set;}
		public AmtDrawIndirectEncoderState[] DrawIndirects { get; internal set;}
		public AmtDrawIndexedEncoderState[] DrawIndexed { get; internal set; }
		public AmtDrawIndexedIndirectEncoderState[] DrawIndexedIndirects { get; internal set;}
		public AmtVertexBufferEncoderState[] VertexBuffers { get; internal set;}
	}
}
