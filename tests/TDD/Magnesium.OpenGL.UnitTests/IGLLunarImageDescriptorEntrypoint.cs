namespace Magnesium.OpenGL.UnitTests
{
    interface IGLLunarImageDescriptorEntrypoint
    {
        long CreateHandle(int textureId, int samplerId);
        void ReleaseHandle(long handle);
        int CreateBuffer(uint overallSize);
        void DestroyBuffer(int bufferId);
        void InsertHandles(int bufferId, uint offset, long[] deltaHandles);
    }
}
