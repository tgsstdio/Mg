using Magnesium;

namespace Magnesium.OpenGL
{
	public interface IGLCmdStencilEntrypoint
	{
		GLGraphicsPipelineStencilState GetDefaultEnums();

		GLQueueRendererStencilState Initialize ();
		void EnableStencilBuffer();
		void DisableStencilBuffer();
		bool IsStencilBufferEnabled { get; }
		void SetStencilWriteMask(int mask);

		void SetFrontFaceCullStencilFunction (MgCompareOp func, int referenceStencil, int stencilMask);
		void SetBackFaceCullStencilFunction(MgCompareOp func, int referenceStencil, int stencilMask);

		void SetFrontFaceStencilOperation(
			MgStencilOp stencilFail,
			MgStencilOp stencilDepthBufferFail,
			MgStencilOp stencilPass);

		void SetBackFaceStencilOperation(
			MgStencilOp stencilFail,
			MgStencilOp stencilDepthBufferFail,
			MgStencilOp stencilPass);

		void SetStencilFunction(
			MgCompareOp stencilFunction,
			int referenceStencil,
			int stencilMask);

		void SetStencilOperation(
			MgStencilOp stencilFail,
			MgStencilOp stencilDepthBufferFail,
			MgStencilOp stencilPass);
	}
}

