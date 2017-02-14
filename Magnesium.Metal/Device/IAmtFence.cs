namespace Magnesium.Metal
{
	public interface IAmtFence : IMgFence
	{
		void Reset();
		void Signal();
		bool AlreadySignalled { get; }
	}
}