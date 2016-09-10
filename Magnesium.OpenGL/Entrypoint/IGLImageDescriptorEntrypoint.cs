namespace Magnesium.OpenGL
{
	public interface IGLImageDescriptorEntrypoint
	{
		ulong CreateHandle (int textureId, int samplerId);
		void ReleaseHandle(ulong handle);
	}

}

