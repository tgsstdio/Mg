namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBlitBag
    {
        public GLCmdEncoderItemCollection<GLCmdCopyBufferRecord> CopyBuffers { get; set; }

        public GLCmdBlitBag()
        {
            CopyBuffers = new GLCmdEncoderItemCollection<GLCmdCopyBufferRecord>();
        }

        public void Clear()
        {
            CopyBuffers.Clear();
        }
    }
}