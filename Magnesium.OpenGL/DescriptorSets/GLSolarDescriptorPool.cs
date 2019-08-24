using Magnesium.OpenGL.Internals;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
    // CODENAME: SOLAR => global texture unit 
    // CODENAME: LUNAR => array of bindless textures via uniform block binding 
    public class GLSolarDescriptorPool : IGLFutureDescriptorPool
    {
        public uint MaxSets { get; set; }

        private readonly ConcurrentBag<IGLFutureDescriptorSet> mAvailableSets;
        public IDictionary<uint, IGLFutureDescriptorSet> AllocatedSets { get; private set; }

        public IGLDescriptorPoolResource<GLTextureSlot> CombinedImageSamplers { get; private set; }
        GLBufferDescriptorPool mBufferPool;

        public GLSolarDescriptorPool(MgDescriptorPoolCreateInfo createInfo)
        {
            MaxSets = createInfo.MaxSets;
            mAvailableSets = new ConcurrentBag<IGLFutureDescriptorSet>();
            for (var i = 1U; i <= MaxSets; i += 1)
            {
                // STARTING FROM 1 - 0 == (default) uint
                mAvailableSets.Add(new GLFutureDescriptorSet(i, this));
            }
            AllocatedSets = new Dictionary<uint, IGLFutureDescriptorSet>();

            var noOfUniformBlocks = 0U;
            uint noOfStorageBuffers = 0U;
            uint noOfCombinedImageSamplers = 0U;

            foreach (var pool in createInfo.PoolSizes)
            {
                switch (pool.Type)
                {
                    case MgDescriptorType.UNIFORM_BUFFER:
                    case MgDescriptorType.UNIFORM_BUFFER_DYNAMIC:
                        noOfUniformBlocks += pool.DescriptorCount;
                        break;
                    case MgDescriptorType.STORAGE_BUFFER:
                    case MgDescriptorType.STORAGE_BUFFER_DYNAMIC:
                        noOfStorageBuffers += pool.DescriptorCount;
                        break;
                    case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
                        noOfCombinedImageSamplers += pool.DescriptorCount;
                        break;
                }
            }

            SetupCombinedImageSamplers(noOfCombinedImageSamplers);
            mBufferPool = new GLBufferDescriptorPool();
            mBufferPool.Setup(noOfUniformBlocks, noOfStorageBuffers);

        }


        void SetupCombinedImageSamplers(uint noOfCombinedImageSamplers)
        {
            var cis = new GLTextureSlot[noOfCombinedImageSamplers];
            for (var i = 0; i < noOfCombinedImageSamplers; i += 1)
            {
                cis[i] = new GLTextureSlot();
            }
            CombinedImageSamplers = new GLPoolResource<GLTextureSlot>(
                noOfCombinedImageSamplers,
                cis);
        }

        public void DestroyDescriptorPool(IMgDevice device, IMgAllocationCallbacks allocator)
        {

        }

        public void ResetResource(GLDescriptorPoolResourceInfo resourceInfo)
        {
            if (resourceInfo != null)
            {
                switch (resourceInfo.GroupType)
                {
                    case GLDescriptorBindingGroup.UniformBuffer:
                        mBufferPool.UniformBuffers.Free(resourceInfo.Ticket);
                        break;
                    case GLDescriptorBindingGroup.CombinedImageSampler:
                        CombinedImageSamplers.Free(resourceInfo.Ticket);
                        break;
                    case GLDescriptorBindingGroup.StorageBuffer:
                        mBufferPool.StorageBuffers.Free(resourceInfo.Ticket);
                        break;
                }
            }
        }

        public MgResult ResetDescriptorPool(IMgDevice device, uint flags)
        {
            foreach (var dSet in AllocatedSets.Values)
            {
                if (dSet != null && dSet.IsValidDescriptorSet)
                {
                    foreach (var resource in dSet.Resources)
                    {
                        ResetResource(resource);
                    }
                    dSet.Invalidate();
                }
            }
            return MgResult.SUCCESS;
        }

        public bool TryTake(out IGLFutureDescriptorSet result)
        {
            return mAvailableSets.TryTake(out result);
        }

        public GLDescriptorPoolAllocationStatus AllocateTicket(
            MgDescriptorType descriptorType,
            uint binding,
            uint count,
            out GLDescriptorPoolResourceInfo resource)
        {
            GLPoolResourceTicket ticket;
            var groupType = GLDescriptorBindingGroup.None;
            switch (descriptorType)
            {
                case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
                    groupType = GLDescriptorBindingGroup.CombinedImageSampler;
                    if (!CombinedImageSamplers.Allocate(count, out ticket))
                    { 
                        resource = null;
                        return GLDescriptorPoolAllocationStatus.FailedAllocation;
                    }
                    break;
                case MgDescriptorType.STORAGE_BUFFER:
                    groupType = GLDescriptorBindingGroup.StorageBuffer;
                    if (!mBufferPool.StorageBuffers.Allocate(count, out ticket))
                    {
                        resource = null;
                        return GLDescriptorPoolAllocationStatus.FailedAllocation;
                    }
                    break;
                case MgDescriptorType.UNIFORM_BUFFER:
                    groupType = GLDescriptorBindingGroup.UniformBuffer;
                    if (!mBufferPool.UniformBuffers.Allocate(count, out ticket))
                    {
                        resource = null;
                        return GLDescriptorPoolAllocationStatus.FailedAllocation;
                    }
                    break;
                default:
                    resource = null;
                    return GLDescriptorPoolAllocationStatus.ResourceNotSupported;
            }

            resource = new GLDescriptorPoolResourceInfo
            {
                Binding = binding,
                DescriptorCount = count,
                GroupType = groupType,
                Ticket = ticket,
            };
            return GLDescriptorPoolAllocationStatus.SuccessfulAllocation;
        }

        public void WritePoolValues(
            MgWriteDescriptorSet change,
            int i,
            GLDescriptorPoolResourceInfo resource,
            uint first,
            uint count)
        {
            switch (change.DescriptorType)
            {
                //case MgDescriptorType.SAMPLER:
                case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
                    UpdateCombinedImageSamplers(change, resource, first, count);
                    break;
                case MgDescriptorType.STORAGE_BUFFER:
                case MgDescriptorType.STORAGE_BUFFER_DYNAMIC:
                    mBufferPool.WriteStorageBufferChanges(
                         change
                        , i
                        , resource
                        , first
                        , count);
                    break;
                case MgDescriptorType.UNIFORM_BUFFER:
                case MgDescriptorType.UNIFORM_BUFFER_DYNAMIC:
                    mBufferPool.WriteUniformBufferChanges(
                         change
                        , i
                        , resource
                        , first
                        , count);
                    break;
                default:
                    throw new PlatformNotSupportedException();
            }
        }

        void UpdateCombinedImageSamplers(
            MgWriteDescriptorSet desc, 
            GLDescriptorPoolResourceInfo resource, 
            uint first,
            uint count)
        {
            if (resource.GroupType == GLDescriptorBindingGroup.CombinedImageSampler)
            {
                for (var j = 0; j < count; j += 1)
                {
                    MgDescriptorImageInfo info = desc.ImageInfo[j];

                    var localSampler = (IGLSampler)info.Sampler;
                    var localView = (IGLImageView)info.ImageView;

                    var index = first + j;
                    CombinedImageSamplers.Items[index].Replace(desc.DstBinding, localView, localSampler);
                }
            }
        }

        public bool GetBufferDescriptor(GLDescriptorBindingGroup groupType, uint i, out GLBufferDescriptor result)
        {
            switch(groupType)
            {
                case GLDescriptorBindingGroup.StorageBuffer:
                    result = mBufferPool.StorageBuffers.Items[i];
                    return true;
                case GLDescriptorBindingGroup.UniformBuffer:
                    result = mBufferPool.UniformBuffers.Items[i];
                    return true;
                default:
                    result = null;
                    return false;
            }
        }
    }
}
