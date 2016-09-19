namespace Magnesium.OpenGL
{
	public interface IGLCmdScissorsEntrypoint
	{
		void ApplyViewports (GLCmdViewportParameter viewports);

		void ApplyScissors(GLCmdScissorParameter scissors);
	}
}

