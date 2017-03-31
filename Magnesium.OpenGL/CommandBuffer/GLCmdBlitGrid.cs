namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBlitGrid
    {
        public GLCmdCopyBufferRecord[] CopyBuffers { get; set; }
        public GLCmdImageInstructionSet[] LoadImageOps { get; set; }
    }
}