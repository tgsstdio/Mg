using System;

namespace Magnesium.OpenGL.UnitTests
{
    internal class MockGLDescriptorSet : IGLFutureDescriptorSet
    {
        public MockGLDescriptorSet()
        {
        }

        public bool IsValidDescriptorSet
        {
            get;
            set;
        }

        public uint Key
        {
            get;
            set;
        }

        public IGLFutureDescriptorPool Parent
        {
            get;
            set;
        }

        public GLDescriptorPoolResourceInfo[] Resources
        {
            get;
            set;
        }

        public bool Equals(IGLFutureDescriptorSet other)
        {
            return ReferenceEquals(this, other);
        }

        public void Initialize(GLDescriptorPoolResourceInfo[] resources)
        {

        }

        public void Invalidate()
        {

        }
    }
}