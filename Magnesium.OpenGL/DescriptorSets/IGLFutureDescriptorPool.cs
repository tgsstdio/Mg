using System.Collections.Generic;

namespace Magnesium.OpenGL
{
    // descriptor pool can contain any implementation
    public interface IGLFutureDescriptorPool
    {
        uint MaxSets { get; }
        IDictionary<uint, IGLFutureDescriptorSet> AllocatedSets { get; }

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
