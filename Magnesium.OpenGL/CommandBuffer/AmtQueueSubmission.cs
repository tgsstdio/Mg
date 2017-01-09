using System.Collections.Generic;

namespace Magnesium.OpenGL
{
    public class AmtQueueSubmission
    {
        public AmtCommandBuffer[] CommandBuffers { get; internal set; }
        public IGLQueueFence OrderFence { get; internal set; }
        public IGLSemaphore[] Signals { get; internal set; }
        public IGLFence Fence { get; internal set; }
    }
}