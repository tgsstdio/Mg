using System.Collections.Generic;

namespace Magnesium.OpenGL.UnitTests
{
    enum GLPoolAllocationStatus
    {
        SuccessfulAllocation,
        FailedAllocation,
        ResourceNotSupported
    }

    // descriptor pool can contain any implementation
    interface IGLFutureDescriptorPool
    {
        uint MaxSets { get; }
        IDictionary<uint, IGLFutureDescriptorSet> AllocatedSets { get; }

        GLPoolAllocationStatus AllocateTicket(
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
