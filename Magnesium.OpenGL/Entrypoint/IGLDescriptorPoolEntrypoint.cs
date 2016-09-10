namespace Magnesium.OpenGL
{
	public interface IGLDescriptorPoolEntrypoint
	{
		IGLDescriptorPool CreatePool(MgDescriptorPoolCreateInfo createInfo);
	}
}

