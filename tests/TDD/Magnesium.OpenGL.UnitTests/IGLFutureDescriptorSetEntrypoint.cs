namespace Magnesium.OpenGL.UnitTests
{
    interface IGLFutureDescriptorSetEntrypoint
    {
        Result Allocate(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets);
        Result Free(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets);
        void Update(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies);
    }
}
