namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLDeviceMemoryEntrypoint : IGLDeviceMemoryEntrypoint
	{
		public IGLDeviceMemory CreateDeviceMemory(MgMemoryAllocateInfo createInfo)
		{
			return new GLDeviceMemory(createInfo);
		}
	}
}

