namespace Magnesium.Utilities
{
    public class MgOptimizedStorage
    {
        public MgOptimizedStorage(
            MgOptimizedStorageBlock[] blocks
        )
        {
            Blocks = blocks;
        }

        public MgOptimizedStorageBlock[] Blocks { get; private set; }

        private bool mIsDisposed = false;
        public void Destroy(IMgDevice device, IMgAllocationCallbacks allocator)
        {
            if (mIsDisposed)
                return;

            if (Blocks != null)
            {
                foreach(var block in Blocks)
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