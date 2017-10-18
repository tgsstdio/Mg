namespace Magnesium.Utilities
{
    public interface IMgOptimizedMeshSegmenter
    {
        MgMeshSegment[] Setup(MgBlockAllocationInfo[] allocations);
    }
}