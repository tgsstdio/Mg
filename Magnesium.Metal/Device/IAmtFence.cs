namespace Magnesium.Metal
{
	public interface IAmtFence : IMgFence
	{
		void Reset(int count);
		void Signal();
		bool AlreadySignalled { get; }
	}
}