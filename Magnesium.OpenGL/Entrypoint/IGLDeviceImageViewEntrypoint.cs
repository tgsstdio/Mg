namespace Magnesium.OpenGL
{
	public interface IGLDeviceImageViewEntrypoint
	{
		int CreateImageView (IGLImage originalImage, MgImageViewCreateInfo pCreateInfo);
		void DeleteImageView(int texture);
	}
}

