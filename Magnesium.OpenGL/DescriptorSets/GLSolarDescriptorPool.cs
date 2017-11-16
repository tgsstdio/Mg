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
        IGLDescriptorPoolResource<GLBufferDescriptor> mStorageBuffers;
        IGLDescriptorPoolResource<GLBufferDescriptor> mUniformBuffers;

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
            SetupUniformBlocks(noOfUniformBlocks);
            SetupStorageBuffers(noOfStorageBuffers);
        }

        void SetupUniformBlocks(uint noOfUniformBlocks)
        {
            var blocks = new GLBufferDescriptor[noOfUniformBlocks];
            for (var i = 0; i < noOfUniformBlocks; i += 1)
            {
                blocks[i] = new GLBufferDescriptor();
            }

            mUniformBuffers = new GLPoolResource<GLBufferDescriptor>(
                noOfUniformBlocks,
                blocks);
        }

        void SetupStorageBuffers(uint noOfStorageBuffers)
        {
            var buffers = new GLBufferDescriptor[noOfStorageBuffers];
            for (var i = 0; i < noOfStorageBuffers; i += 1)
            {
                buffers[i] = new GLBufferDescriptor();
            }

            mStorageBuffers = new GLPoolResource<GLBufferDescriptor>(
                noOfStorageBuffers,
                buffers);
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
                        mUniformBuffers.Free(resourceInfo.Ticket);
                        break;
                    case GLDescriptorBindingGroup.CombinedImageSampler:
                        CombinedImageSamplers.Free(resourceInfo.Ticket);
                        break;
                    case GLDescriptorBindingGroup.StorageBuffer:
                        mStorageBuffers.Free(resourceInfo.Ticket);
                        break;
                }
            }
        }

        public Result ResetDescriptorPool(IMgDevice device, uint flags)
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
            return Result.SUCCESS;
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
                    if (!mStorageBuffers.Allocate(count, out ticket))
                    {
                        resource = null;
                        return GLDescriptorPoolAllocationStatus.FailedAllocation;
                    }
                    break;
                case MgDescriptorType.UNIFORM_BUFFER:
                    groupType = GLDescriptorBindingGroup.UniformBuffer;
                    if (!mUniformBuffers.Allocate(count, out ticket))
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
                    WriteBufferChanges(
                        GLDescriptorBindingGroup.StorageBuffer,
                        MgBufferUsageFlagBits.STORAGE_BUFFER_BIT,
                        MgDescriptorType.STORAGE_BUFFER_DYNAMIC,
                        mStorageBuffers,
                         change
                        , i
                        , resource
                        , first
                        , count);
                    break;
                case MgDescriptorType.UNIFORM_BUFFER:
                case MgDescriptorType.UNIFORM_BUFFER_DYNAMIC:
                    WriteBufferChanges(
                        GLDescriptorBindingGroup.UniformBuffer,
                        MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
                        MgDescriptorType.UNIFORM_BUFFER_DYNAMIC,
                        mUniformBuffers,
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

        void WriteBufferChanges(
            GLDescriptorBindingGroup bindingGroup,
            MgBufferUsageFlagBits isBufferFlags,
            MgDescriptorType dynamicBufferType,
            IGLDescriptorPoolResource<GLBufferDescriptor> collection,
            MgWriteDescriptorSet change,
            int i,
            GLDescriptorPoolResourceInfo resource,
            uint first,
            uint count
        )
        {
            if (resource.GroupType == bindingGroup)
            {
                for (var j = 0; j < count; j += 1)
                {
                    var info = change.BufferInfo[j];
                    var buf = (IGLBuffer)info.Buffer;

                    if (buf != null && ((buf.Usage & isBufferFlags) == isBufferFlags))
                    {
                        var index = first + j;
                        var bufferDesc = collection.Items[index];
                        bufferDesc.BufferId = buf.BufferId;

                        ValidateOffset(i, j, info);
                        ValidateRange(i, j, info);

                        bufferDesc.Offset = (long)info.Offset;
                        // need to pass in whole 
                        bufferDesc.Size = (int)info.Range;
                        bufferDesc.IsDynamic = (change.DescriptorType == dynamicBufferType);
                    }

                }
            }
        }

        private static void ValidateRange(int i, int j, MgDescriptorBufferInfo info)
        {
            // CROSS PLATFORM ISSUE : VK_WHOLE_SIZE == ulong.MaxValue
            if (info.Range == ulong.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    "Mg.OpenGL : Cannot accept pDescriptorWrites[" + i
                    + "].BufferInfo[" + j +
                    "].Range == ulong.MaxValue (VK_WHOLE_SIZE). Please use actual size of buffer instead.");
            }

            if (info.Range > (ulong)int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    "pDescriptorWrites[" + i
                    + "].BufferInfo[" + j + "].Range is > int.MaxValue");
            }
        }

        private static void ValidateOffset(int i, int j, MgDescriptorBufferInfo info)
        {
            if (info.Offset > (ulong)long.MaxValue)
            {
                throw new ArgumentOutOfRangeException("pDescriptorWrites[" + i
                    + "].BufferInfo[" + j + "].Offset is > long.MaxValue");
            }
        }
    }
}
