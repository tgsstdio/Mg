namespace Magnesium.OpenGL.Internals
{
    public class GLCmdCommandRecording
	{
		public GLCmdGraphicsRecording Graphics { get; set; }
		public GLCmdBlitRecording Blit { get; set; }
		public GLCmdComputeRecording Compute { get; set; }
	}
}
