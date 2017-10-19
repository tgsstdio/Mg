using System;
using System.Collections.Generic;

namespace Magnesium.Utilities
{
    public class MgOptimizedMeshBuilder
    {
        private IMgGraphicsConfiguration mConfiguration;
        private IMgOptimizedMeshSegmenter mSplitter;
        private IMgOptimizedMeshSegmentVerifier mVerifier;
        public MgOptimizedMeshBuilder(
            IMgGraphicsConfiguration config,
            IMgOptimizedMeshSegmenter segmenter,
            IMgOptimizedMeshSegmentVerifier verifier
        )
        {
            mConfiguration = config;
            mSplitter = segmenter;
            mVerifier = verifier;
        }

        public MgOptimizedMesh Build(MgOptimizedMeshCreateInfo createInfo)
        {
            // setup 
            Validate(createInfo);
            var optimalLayout = mSplitter.Setup(createInfo.Allocations);

            var bufferInstances = mVerifier.Revise(optimalLayout, createInfo);

            return InitializeMesh(createInfo, bufferInstances);
        }

        public MgOptimizedMesh Build(MgOptimizedMeshCreateInfo createInfo, MgBufferInstance[] bufferInstances)
        {
            Validate(createInfo);
            return InitializeMesh(createInfo, bufferInstances);
        }

        private MgOptimizedMesh InitializeMesh(MgOptimizedMeshCreateInfo createInfo, MgBufferInstance[] bufferInstances)
        {
            var attributes = RemapAttributes(bufferInstances, createInfo);

            var blocks = AllocateBlocks(bufferInstances, createInfo);

            return new MgOptimizedMesh(blocks, attributes);
        }

        private MgOptimizedMeshAllocation[] RemapAttributes(MgBufferInstance[] bufferInstances, MgOptimizedMeshCreateInfo createInfo)
        {
            var attributes = new MgOptimizedMeshAllocation[createInfo.Allocations.Length];

            for(var i = 0U; i < bufferInstances.Length; i += 1)
            {
                var instance = bufferInstances[i];

                foreach (var mapping in instance.Mappings)
                {
                    attributes[mapping.Index] = new MgOptimizedMeshAllocation
                    {
                       Index = mapping.Index,
                       InstanceIndex = i,
                       Offset = mapping.Offset,
                       Size = mapping.Size,
                       Usage = mapping.Usage,                       
                    };
                }    
            }

            return attributes;
        }

        private void Validate(MgOptimizedMeshCreateInfo createInfo)
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

        private MgOptimizedMeshInstance[] AllocateBlocks(MgBufferInstance[] bufferInstances, MgOptimizedMeshCreateInfo createInfo)
        {
            var memoryBlocks = new List<MgOptimizedMeshInstance>();
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

                var block = new MgOptimizedMeshInstance
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
