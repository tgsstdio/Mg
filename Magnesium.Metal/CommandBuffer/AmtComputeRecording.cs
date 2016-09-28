using Metal;

namespace Magnesium.Metal
{
	public class AmtComputeRecording
	{
		public IMTLComputeCommandEncoder Encoder { get; internal set; }
		public AmtComputeEncoderItemGrid Grid { get; set;}
	}
}