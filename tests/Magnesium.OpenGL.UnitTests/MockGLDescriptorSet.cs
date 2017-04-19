using System;

namespace Magnesium.OpenGL.UnitTests
{
    internal class MockGLDescriptorSet : IGLDescriptorSet
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

        public IGLNextDescriptorPool Parent
        {
            get;
            set;
        }

        public GLDescriptorPoolResourceInfo[] Resources
        {
            get;
            set;
        }

        public bool Equals(IGLDescriptorSet other)
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