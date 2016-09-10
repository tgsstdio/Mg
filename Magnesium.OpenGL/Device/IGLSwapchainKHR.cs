namespace Magnesium.OpenGL
{
	public interface IGLSwapchainKHR
	{
		uint GetNextImage ();
		void SwapBuffers();
	}
}

