using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL.UnitTests
{
    internal class MockGLDescriptorPool : IGLNextDescriptorPool
    {
        public uint MaxSets
        {
            get;
            set;
        }

        public IDictionary<uint, IGLDescriptorSet> AllocatedSets
        {
            get;
            set;
        }

        public IGLDescriptorPoolResource<GLImageDescriptor> CombinedImageSamplers
        {
            get;
            set;
        }

        public IGLDescriptorPoolResource<GLBufferDescriptor> UniformBuffers
        {
            get;
            set;
        }

        public IGLDescriptorPoolResource<GLBufferDescriptor> StorageBuffers
        {
            get;
            set;
        }

        public void ResetResource(GLDescriptorPoolResourceInfo resource)
        {

        }

        public IGLDescriptorSet CurrentDescriptorSet { get; set; }
        public bool TryTake(out IGLDescriptorSet result)
        {
            result = CurrentDescriptorSet;
            return true;
        }

        public void DestroyDescriptorPool(IMgDevice device, IMgAllocationCallbacks allocator)
        {

        }

        public Result ResetDescriptorPool(IMgDevice device, uint flags)
        {
            throw new NotImplementedException();
        }
    }
}