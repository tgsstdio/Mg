using Metal;

namespace Magnesium.Metal
{
	public class AmtComputeRecording
	{
		public IMTLComputeCommandEncoder Encoder { get; internal set; }
		public AmtComputeGrid Grid { get; set;}
	}
}