namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBlitBag
    {
        public GLCmdEncoderItemCollection<GLCmdCopyBufferRecord> CopyBuffers { get; set; }
        public GLCmdEncoderItemCollection<GLCmdTexImageData> ImageData { get; set; }

        public GLCmdBlitBag()
        {
            CopyBuffers = new GLCmdEncoderItemCollection<GLCmdCopyBufferRecord>();
            ImageData = new GLCmdEncoderItemCollection<GLCmdTexImageData>();
        }

        public void Clear()
        {
            CopyBuffers.Clear();
            ImageData.Clear();
        }
    }
}