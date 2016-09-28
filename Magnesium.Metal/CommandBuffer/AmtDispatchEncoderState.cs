using Metal;

namespace Magnesium.Metal
{
	public class AmtDispatchEncoderState
	{
		public MTLSize GroupSize { get; internal set; }
		public MTLSize ThreadsPerGroupSize { get; internal set; }
	}
}