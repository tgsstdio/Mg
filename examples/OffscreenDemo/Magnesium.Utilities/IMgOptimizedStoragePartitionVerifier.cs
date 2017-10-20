namespace Magnesium.Utilities
{
    public interface IMgOptimizedStoragePartitionVerifier
    {
        MgStorageBufferInstance[] Revise(MgStorageBlockInfo[] segments, MgOptimizedStorageCreateInfo createInfo);
    }
}
