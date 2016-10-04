namespace Magnesium.Metal
{
	public class AmtBlitGrid
	{
		public AmtBlitCopyBufferRecord[] CopyBuffers { get; set;}
		public AmtBlitCopyBufferToImageRecord[] CopyBufferToImages { get; set;}
		public AmtBlitCopyImageRecord[] CopyImages { get; set;}
	}
}