namespace Magnesium.OpenGL
{
	public interface IGLDeviceMemoryEntrypoint
	{
		IGLDeviceMemory CreateDeviceMemory(MgMemoryAllocateInfo createInfo);
	}
}

