namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLSemaphoreEntrypoint : IGLSemaphoreEntrypoint
	{
		#region IGLSemaphoreGenerator implementation
		public IGLSemaphore CreateSemaphore ()
		{
			return new GLQueueSemaphore ();
		}
		#endregion
	}
}
