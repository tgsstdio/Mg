namespace Magnesium.OpenGL
{
	public interface IGLDescriptorSetEntrypoint
	{
		Result Allocate(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets);
		Result Free(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets);
        void Update(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies);
    }
}