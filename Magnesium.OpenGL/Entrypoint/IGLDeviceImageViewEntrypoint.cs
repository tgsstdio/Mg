namespace Magnesium.OpenGL
{
	public interface IGLDeviceImageViewEntrypoint
	{
		int CreateImageView (GLImage originalImage, MgImageViewCreateInfo pCreateInfo);
		void DeleteImageView(int texture);
	}
}

