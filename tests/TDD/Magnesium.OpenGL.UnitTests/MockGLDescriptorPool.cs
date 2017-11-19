using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL.UnitTests
{
    internal class MockGLDescriptorPool : IGLFutureDescriptorPool
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

        public IGLDescriptorPoolResource<GLTextureSlot> CombinedImageSamplers
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

        IDictionary<uint, IGLFutureDescriptorSet> IGLFutureDescriptorPool.AllocatedSets => throw new NotImplementedException();

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

        public bool GetBufferDescriptor(GLDescriptorBindingGroup groupType, uint i, out GLBufferDescriptor result)
        {
            switch (groupType)
            {
                case GLDescriptorBindingGroup.StorageBuffer:
                    result = StorageBuffers.Items[i];
                    return true;
                case GLDescriptorBindingGroup.UniformBuffer:
                    result = UniformBuffers.Items[i];
                    return true;
                default:
                    result = null;
                    return false;
            }
        }

        public GLDescriptorPoolAllocationStatus AllocateTicket(MgDescriptorType descriptorType, uint binding, uint count, out GLDescriptorPoolResourceInfo resource)
        {
            throw new NotImplementedException();
        }

        public bool TryTake(out IGLFutureDescriptorSet result)
        {
            throw new NotImplementedException();
        }

        public void WritePoolValues(MgWriteDescriptorSet desc, int i, GLDescriptorPoolResourceInfo ticket, uint first, uint count)
        {
            throw new NotImplementedException();
        }
    }
}