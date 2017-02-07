namespace Magnesium.OpenGL.Internals
{
    public class AmtCommandRecording
	{
		public AmtGraphicsRecording Graphics { get; set; }
		public AmtBlitRecording Blit { get; set; }
		public AmtComputeRecording Compute { get; set; }
	}
}
