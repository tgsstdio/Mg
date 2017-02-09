namespace Magnesium.OpenGL
{
	public interface IGLImageDescriptorEntrypoint
	{
		long CreateHandle (int textureId, int samplerId);
		void ReleaseHandle(long handle);
	}

}

