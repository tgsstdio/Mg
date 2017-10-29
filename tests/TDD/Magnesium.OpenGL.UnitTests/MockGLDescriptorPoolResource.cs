using System;

namespace Magnesium.OpenGL.UnitTests
{
    internal class MockGLDescriptorPoolResource<T> : IGLDescriptorPoolResource<T>
    {
        public uint Count
        {
            get
            {
                return (uint) Items.Length;
            }
        }

        public T[] Items
        {
            get;
            set;
        }

        public bool Allocate(uint request, out GLPoolResourceTicket ticket)
        {
            throw new NotImplementedException();
        }

        public bool Free(GLPoolResourceTicket ticket)
        {
            throw new NotImplementedException();
        }
    }
}