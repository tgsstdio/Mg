namespace Magnesium.OpenGL.Internals
{
    public struct GLCmdLoadColorAttachment
    {
        public uint ColorAttachment { get; set; }
        public GLClearAttachmentType AttachmentType { get; set; }
        public GLCmdClearValueArrayItem ClearValue { get; set; }
    }

    public class GLCmdSubpassOperation
    {
        public int FBO { get; set; }
        public GLCmdLoadColorAttachment[] Loads { get; set; }
        public GLQueueClearBufferMask ClearMask { get; internal set; }
    }
}
