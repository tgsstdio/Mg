using Metal;

namespace Magnesium.Metal
{
	public class AmtDispatchRecord
	{
		public MTLSize GroupSize { get; internal set; }
		public MTLSize ThreadsPerGroupSize { get; internal set; }
	}
}