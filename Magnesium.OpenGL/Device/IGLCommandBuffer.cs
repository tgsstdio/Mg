namespace Magnesium.OpenGL.Internals
{
    public interface IGLCommandBuffer : IMgCommandBuffer
    {
        void ResetAllData();
        bool IsQueueReady { get; }
        GLCmdCommandBufferRecord Record { get; }
    }
}