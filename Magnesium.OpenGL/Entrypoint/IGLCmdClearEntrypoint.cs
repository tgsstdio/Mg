using Magnesium;

namespace Magnesium.OpenGL
{
	public interface IGLCmdClearEntrypoint
	{
		GLQueueRendererClearValueState Initialize ();
		void ClearBuffers (GLQueueClearBufferMask combinedMask);
		void SetClearStencilValue (uint stencil);
		void SetClearDepthValue (float value);
		void SetClearColor (MgColor4f clearValue);
	}
}

