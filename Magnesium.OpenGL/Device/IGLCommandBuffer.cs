using System;

namespace Magnesium.OpenGL
{
    public interface IGLCommandBuffer : IMgCommandBuffer
    {
        void ResetAllData();
        bool IsQueueReady { get; }
        AmtCommandBufferRecord Record { get; }
    }
}