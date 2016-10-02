using Metal;

namespace Magnesium.Metal
{
	public interface IAmtQueueRenderer
	{
		IMTLCommandBuffer[] Render(GLQueueSubmission request);
	}
}