using Magnesium;

namespace Magnesium.OpenGL
{
	public interface IGLCmdClearEntrypoint
	{
		GLClearValueState Initialize ();
		void ClearBuffers (GLQueueClearBufferMask combinedMask);
		void SetClearStencilValue (uint stencil);
		void SetClearDepthValue (float value);
		void SetClearColor (MgColor4f clearValue);

        void ClearFramebufferiv(uint index, MgVec4i clearValue);
        void ClearFramebufferuiv(uint index, MgVec4Ui clearValue);
        void ClearFramebufferfv(uint index, MgColor4f clearValue);
        void ClearFramebufferDepthStencil(MgClearDepthStencilValue clearValue);
	}
}

