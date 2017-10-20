using System;
using System.Collections.Generic;

namespace Magnesium.Utilities
{
    public class MgOptimizedStorageBuilder
    {
        private IMgGraphicsConfiguration mConfiguration;
        private IMgOptimizedStoragePartitioner mSplitter;
        private IMgOptimizedStoragePartitionVerifier mVerifier;
        public MgOptimizedStorageBuilder(
            IMgGraphicsConfiguration config,
            IMgOptimizedStoragePartitioner segmenter,
            IMgOptimizedStoragePartitionVerifier verifier
        )
        {
            mConfiguration = config;
            mSplitter = segmenter;
            mVerifier = verifier;
        }

        public MgOptimizedStorage Build(MgOptimizedStorageCreateInfo createInfo)
        {
            // setup 
            Validate(createInfo);
            var optimalLayout = mSplitter.Setup(createInfo.Allocations);

            var bufferInstances = mVerifier.Revise(optimalLayout, createInfo);

            return InitializeMesh(createInfo, bufferInstances);
        }

        public MgOptimizedStorage Build(MgOptimizedStorageCreateInfo createInfo, MgStorageBufferInstance[] bufferInstances)
        {
            Validate(createInfo);
            return InitializeMesh(createInfo, bufferInstances);
        }

        private MgOptimizedStorage InitializeMesh(MgOptimizedStorageCreateInfo createInfo, MgStorageBufferInstance[] bufferInstances)
        {
            var attributes = RemapAttributes(bufferInstances, createInfo);

            var blocks = AllocateBlocks(bufferInstances, createInfo);

            return new MgOptimizedStorage(blocks, attributes);
        }

        private MgOptimizedStorageAllocation[] RemapAttributes(MgStorageBufferInstance[] bufferInstances, MgOptimizedStorageCreateInfo createInfo)
        {
            var attributes = new MgOptimizedStorageAllocation[createInfo.Allocations.Length];

            for(var i = 0U; i < bufferInstances.Length; i += 1)
            {
                var instance = bufferInstances[i];

                foreach (var mapping in instance.Mappings)
                {
                    attributes[mapping.Index] = new MgOptimizedStorageAllocation
                    {
                       AllocationIndex = mapping.Index,
                       BlockIndex = i,
                       Offset = mapping.Offset,
                       Size = mapping.Size,
                       Usage = mapping.Usage,                       
                    };
                }    
            }

            return attributes;
        }

        private void Validate(MgOptimizedStorageCreateInfo createInfo)
        {
            if (createInfo.Allocations == null)
            {
                throw new ArgumentNullException("createInfo.Allocations");
            }

            if (createInfo.Allocations.Length <= 0)
            {
                throw new ArgumentOutOfRangeException("createInfo.Allocations must be greater than zero");
            }
        }

        private MgOptimizedStorageBlock[] AllocateBlocks(MgStorageBufferInstance[] bufferInstances, MgOptimizedStorageCreateInfo createInfo)
        {
            var memoryBlocks = new List<MgOptimizedStorageBlock>();
            foreach (var instance in bufferInstances)
            {
                var pAllocateInfo = new MgMemoryAllocateInfo
                {                    
                    AllocationSize = instance.AllocationSize,
                    MemoryTypeIndex = instance.TypeIndex,
                };

                var err = mConfiguration.Device.AllocateMemory(pAllocateInfo, 
                    null,
                    out IMgDeviceMemory pMemory);

                if (err != Result.SUCCESS)
                {
                    // START OVER 
                    throw new InvalidOperationException(err.ToString()); ;
                }

                var children = new List<uint>();
                foreach(var mapping in instance.Mappings)
                {
                    children.Add(mapping.Index);
                }

                var block = new MgOptimizedStorageBlock
                {
                    Buffer = instance.Buffer,
                    DeviceMemory = pMemory,
                    AllocationSize = instance.AllocationSize,
                    MemoryOffset = 0UL,
                    Usage = instance.Usage,
                    MemoryPropertyFlags = instance.MemoryPropertyFlags,
                    PackingOrder = children.ToArray(),
                };

                block.Buffer.BindBufferMemory(
                    mConfiguration.Device,
                    block.DeviceMemory,
                    block.MemoryOffset);

                memoryBlocks.Add(block);
            }
            return memoryBlocks.ToArray();
        }
    }
}
