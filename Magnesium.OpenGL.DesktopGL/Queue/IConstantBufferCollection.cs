
namespace Magnesium.OpenGL.DesktopGL
{
	public interface IConstantBufferCollection
	{
		//IConstantBuffer[] Filter (Mesh mesh, EffectPass pass, int options);
		void Add(IConstantBuffer b);
	}
}

