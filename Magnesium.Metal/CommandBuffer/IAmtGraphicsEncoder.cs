namespace Magnesium.Metal
{
	public interface IAmtGraphicsEncoder
	{
		void Clear();
		void BindPipeline(IMgPipeline pipeline);
		void SetViewports(uint firstViewport, MgViewport[] pViewports);
		void SetScissor(uint firstScissor, MgRect2D[] pScissors);
		void SetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);
		void SetBlendConstants(MgColor4f blendConstants);
		void SetStencilCompareMask(MgStencilFaceFlagBits faceMask, uint compareMask);
		void SetStencilWriteMask(MgStencilFaceFlagBits faceMask, uint writeMask);
		void SetStencilReference(MgStencilFaceFlagBits faceMask, uint reference);
		void BeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents);
		void EndRenderPass();
		void NextSubpass(MgSubpassContents contents);
	}
}