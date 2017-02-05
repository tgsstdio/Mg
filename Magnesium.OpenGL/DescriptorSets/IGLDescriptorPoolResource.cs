namespace Magnesium.OpenGL
{
	public interface IGLDescriptorPoolResource<T>
	{
		T[] Items { get; }
		uint Count { get; }
		bool Allocate(uint request, out GLPoolResourceTicket ticket);
		bool Free(GLPoolResourceTicket ticket);
	}
}