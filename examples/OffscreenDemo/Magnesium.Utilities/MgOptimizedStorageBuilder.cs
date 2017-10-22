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

        public MgOptimizedStorageContainer Build(MgOptimizedStorageCreateInfo createInfo)
        {
            var bufferInstances = PrepareInstances(createInfo);
            var storageMap = MapAllocations(createInfo, bufferInstances);
            var storage = AllocateBlocks(bufferInstances);

            return new MgOptimizedStorageContainer
            {
                Storage = storage,
                Map = storageMap,
            };
        }

        public MgStorageBufferInstance[] PrepareInstances(MgOptimizedStorageCreateInfo createInfo)
        {
            // setup 
            Validate(createInfo);
            var optimalLayout = mSplitter.Setup(createInfo.Allocations);

            return mVerifier.Revise(createInfo, optimalLayout);
        }

        public MgOptimizedStorageMap MapAllocations(MgOptimizedStorageCreateInfo createInfo, MgStorageBufferInstance[] bufferInstances)
        {
            Validate(createInfo);
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

            return new MgOptimizedStorageMap { Allocations = attributes };
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

        public MgOptimizedStorage AllocateBlocks(MgStorageBufferInstance[] bufferInstances)
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
            return new MgOptimizedStorage(memoryBlocks.ToArray());
        }
    }
}
