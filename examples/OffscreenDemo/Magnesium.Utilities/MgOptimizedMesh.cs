using Magnesium;

namespace Magnesium.Utilities
{
    public class MgOptimizedMesh
    {
        public MgOptimizedMesh(
            MgOptimizedMeshInstance[] instances,
            MgOptimizedMeshAllocation[] allocation
        )
        {
            Instances = instances;
            Allocations = allocation;
        }

        public MgOptimizedMeshInstance[] Instances { get; private set; }
        public MgOptimizedMeshAllocation[] Allocations { get; private set; }

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