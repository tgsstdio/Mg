﻿namespace Magnesium.Toolkit
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

		void SetImageLayout (
			IMgCommandBuffer cmdbuffer, 
			IMgImage image, 
			MgImageAspectFlagBits aspectMask, 
			MgImageLayout oldImageLayout, 
			MgImageLayout newImageLayout,
			uint mipLevel,
			uint mipLevelCount		
		);
	}
}

