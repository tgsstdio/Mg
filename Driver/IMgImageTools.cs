namespace Magnesium
{
	public interface IMgImageTools
	{
		// Fixed sub resource on first mip level and layer
		void SetImageLayout(
			IMgCommandBuffer cmdbuffer,
			IMgImage image,
			MgImageAspectFlagBits aspectMask,
			MgImageLayout oldImageLayout,
			MgImageLayout newImageLayout);
	}
}

