namespace Magnesium.OpenGL
{
	public interface IGLDescriptorPoolEntrypoint
	{
		IGLNextDescriptorPool CreatePool(MgDescriptorPoolCreateInfo createInfo);
	}
}

