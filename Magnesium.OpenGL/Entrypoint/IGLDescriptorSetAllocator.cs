namespace Magnesium.OpenGL
{
	public interface IGLDescriptorSetEntrypoint
	{
		MgResult Allocate(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets);
		MgResult Free(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets);
        void Update(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies);
    }
}