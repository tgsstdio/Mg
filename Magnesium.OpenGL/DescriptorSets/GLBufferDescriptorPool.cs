using Magnesium.OpenGL.Internals;
using System;

namespace Magnesium.OpenGL
{
    public class GLBufferDescriptorPool
    {
        public IGLDescriptorPoolResource<GLBufferDescriptor> StorageBuffers { get; private set; }
        public IGLDescriptorPoolResource<GLBufferDescriptor> UniformBuffers { get; private set; }

        public void Setup(uint noOfUniformBlocks, uint noOfStorageBuffers)
        {
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

            UniformBuffers = new GLPoolResource<GLBufferDescriptor>(
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

            StorageBuffers = new GLPoolResource<GLBufferDescriptor>(
                noOfStorageBuffers,
                buffers);
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

        public void WriteStorageBufferChanges(MgWriteDescriptorSet change, int i, GLDescriptorPoolResourceInfo resource, uint first, uint count)
        {
            WriteBufferChanges(
                GLDescriptorBindingGroup.StorageBuffer,
                MgBufferUsageFlagBits.STORAGE_BUFFER_BIT,
                MgDescriptorType.STORAGE_BUFFER_DYNAMIC,
                StorageBuffers,
                 change
                , i
                , resource
                , first
                , count);
        }

        internal void WriteUniformBufferChanges(MgWriteDescriptorSet change, int i, GLDescriptorPoolResourceInfo resource, uint first, uint count)
        {
            WriteBufferChanges(
                GLDescriptorBindingGroup.UniformBuffer,
                MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
                MgDescriptorType.UNIFORM_BUFFER_DYNAMIC,
                UniformBuffers,
                 change
                , i
                , resource
                , first
                , count);
        }
    }
}
