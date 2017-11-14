using System;

namespace Magnesium.OpenGL.UnitTests
{
    interface IGLFutureDescriptorSet : IMgDescriptorSet, IEquatable<IGLFutureDescriptorSet>
    {
        uint Key { get; }
        IGLFutureDescriptorPool Parent { get; }
        GLDescriptorPoolResourceInfo[] Resources { get; }

        void Initialize(GLDescriptorPoolResourceInfo[] resources);
        bool IsValidDescriptorSet { get; }
        void Invalidate();
    }
}
