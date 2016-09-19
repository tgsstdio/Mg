namespace Magnesium.OpenGL
{
	public interface IGLErrorHandler
	{
		void CheckGLError();
		void LogGLError(string location);
		void Trace(string message);
	}
}

