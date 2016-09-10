namespace Magnesium.OpenGL
{
	public interface IGLCmdBufferStore<TData>
	{
		void Add (TData item);
		int? LastIndex();
		bool LastValue (ref TData item);
		TData At(int index);
		int Count { get; }
	}
}

