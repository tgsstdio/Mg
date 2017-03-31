namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBlitBag
    {
        public GLCmdEncoderItemCollection<GLCmdCopyBufferRecord> CopyBuffers { get; set; }
        public GLCmdEncoderItemCollection<GLCmdImageInstructionSet> LoadImageOps { get; set; }

        public GLCmdBlitBag()
        {
            CopyBuffers = new GLCmdEncoderItemCollection<GLCmdCopyBufferRecord>();
            LoadImageOps = new GLCmdEncoderItemCollection<GLCmdImageInstructionSet>();
        }

        public void Clear()
        {
            CopyBuffers.Clear();
            LoadImageOps.Clear();
        }
    }
}