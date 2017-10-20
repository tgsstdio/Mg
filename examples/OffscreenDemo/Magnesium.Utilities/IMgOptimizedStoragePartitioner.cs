namespace Magnesium.Utilities
{
    public interface IMgOptimizedStoragePartitioner
    {
        MgStorageBlockInfo[] Setup(MgStorageBlockAllocationInfo[] allocations);
    }
}