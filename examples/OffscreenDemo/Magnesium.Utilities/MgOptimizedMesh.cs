using Magnesium;

namespace Magnesium.Utilities
{
    public class MgOptimizedMesh
    {
        public MgOptimizedMesh(
            MgOptimizedMeshMemoryBlock[] blocks,
            MgOptimizedMeshAttribute[] attributes
        )
        {
            Instances = blocks;
            Allocations = attributes;
        }

        public MgOptimizedMeshMemoryBlock[] Instances { get; private set; }
        public MgOptimizedMeshAttribute[] Allocations { get; private set; }

        private bool mIsDisposed = false;
        public void Destroy(IMgDevice device, IMgAllocationCallbacks allocator)
        {
            if (mIsDisposed)
                return;

            if (Instances != null)
            {
                foreach(var block in Instances)
                {
                    if (block != null)
                    {
                        if (block.DeviceMemory != null)
                            block.DeviceMemory.FreeMemory(device, allocator);

                        if (block.Buffer != null)
                            block.Buffer.DestroyBuffer(device, allocator);
                    }
                }
            }

            mIsDisposed = true;
        }
    }
}