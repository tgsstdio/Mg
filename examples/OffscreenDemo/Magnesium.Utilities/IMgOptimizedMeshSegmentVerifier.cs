namespace Magnesium.Utilities
{
    public interface IMgOptimizedMeshSegmentVerifier
    {
        MgBufferInstance[] Revise(MgMeshSegment[] segments, MgOptimizedMeshCreateInfo createInfo);
    }
}
