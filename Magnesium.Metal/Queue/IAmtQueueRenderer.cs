using Metal;

namespace Magnesium.Metal
{
	public interface IAmtQueueRenderer
	{
		void Render(AmtQueueSubmission request);
	}
}