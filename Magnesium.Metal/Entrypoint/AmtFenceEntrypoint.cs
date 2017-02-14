namespace Magnesium.Metal
{
	public class AmtFenceEntrypoint : IAmtFenceEntrypoint
	{
		public IAmtFence CreateFence()
		{
			return new AmtFence();
		}
	}
}
