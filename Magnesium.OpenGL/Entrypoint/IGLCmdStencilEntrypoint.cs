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
		void SetStencilWriteMask(MgStencilFaceFlagBits face, uint mask);

		void SetFrontFaceCullStencilFunction (MgCompareOp func, int referenceStencil, uint compare);
		void SetBackFaceCullStencilFunction(MgCompareOp func, int referenceStencil, uint compare);

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
			uint compare);

		void SetStencilOperation(
			MgStencilOp stencilFail,
			MgStencilOp stencilDepthBufferFail,
			MgStencilOp stencilPass);
	}
}

