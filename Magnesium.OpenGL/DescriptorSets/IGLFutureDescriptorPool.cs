using System.Collections.Generic;

namespace Magnesium.OpenGL
{
    // descriptor pool can contain any implementation
    public interface IGLFutureDescriptorPool : IMgDescriptorPool
    {
        uint MaxSets { get; }
        IDictionary<uint, IGLFutureDescriptorSet> AllocatedSets { get; }

         bool GetBufferDescriptor(
            GLDescriptorBindingGroup groupType,
            uint i,
            out GLBufferDescriptor result
            );

        GLDescriptorPoolAllocationStatus AllocateTicket(
            MgDescriptorType descriptorType,
            uint binding,
            uint count,
            out GLDescriptorPoolResourceInfo resource);
        void ResetResource(GLDescriptorPoolResourceInfo resource);

        bool TryTake(out IGLFutureDescriptorSet result);

        void WritePoolValues(MgWriteDescriptorSet desc,
            int i,
            GLDescriptorPoolResourceInfo ticket,
            uint first,
            uint count);
    }
}
