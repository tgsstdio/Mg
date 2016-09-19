namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLBufferEntrypoint : IGLBufferEntrypoint
	{
		public IGLBuffer CreateBuffer(MgBufferCreateInfo createInfo)
		{
			return new FullGLBuffer(createInfo);
		}
	}
}

