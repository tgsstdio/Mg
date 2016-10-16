using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsGrid
	{
		public MTLRenderPassDescriptor[] RenderPasses { get; internal set;}
		public MgColor4f[] BlendConstants { get; internal set; }
		public AmtPipelineStateRecord[] Pipelines { get; internal set; }
		public IMTLDepthStencilState[] DepthStencilStates { get; internal set; }
		public AmtDepthBiasRecord[] DepthBias { get; internal set; }
		public AmtStencilReferenceRecord[] StencilReferences { get; internal set; }
		public MTLScissorRect[] Scissors { get; internal set;}
		public MTLViewport[] Viewports { get; internal set; }
		public AmtDrawRecord[] Draws { get; internal set;}
		public AmtDrawIndirectRecord[] DrawIndirects { get; internal set;}
		public AmtDrawIndexedRecord[] DrawIndexed { get; internal set; }
		public AmtDrawIndexedIndirectRecord[] DrawIndexedIndirects { get; internal set;}
		public AmtVertexBufferRecord[] VertexBuffers { get; internal set;}
	}
}
