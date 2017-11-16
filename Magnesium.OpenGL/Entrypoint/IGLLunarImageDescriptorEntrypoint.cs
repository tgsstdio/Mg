namespace Magnesium.OpenGL
{
    public interface IGLLunarImageDescriptorEntrypoint
    {
        long CreateHandle(int textureId, int samplerId);
        void ReleaseHandle(long handle);
        uint CreateBuffer(uint overallSize);
        void DestroyBuffer(uint bufferId);
        void InsertHandles(uint bufferId, uint offset, long[] deltaHandles);
    }
}
