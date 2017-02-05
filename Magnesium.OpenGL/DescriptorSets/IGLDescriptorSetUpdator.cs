namespace Magnesium.OpenGL
{
	public interface IGLDescriptorSetUpdator
	{
		void Update(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies);
	}
}