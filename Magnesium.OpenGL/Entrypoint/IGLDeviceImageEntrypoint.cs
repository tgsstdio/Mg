
namespace Magnesium.OpenGL
{
	public interface IGLDeviceImageEntrypoint
	{
		void DeleteImage (int textureId);

		int CreateTextureStorage1D (int levels, MgFormat format, int width);

		int CreateTextureStorage2D (int levels, MgFormat format, int width, int height);

		int CreateTextureStorage3D (int levels, MgFormat format, int width, int height, int depth);
	}
}

