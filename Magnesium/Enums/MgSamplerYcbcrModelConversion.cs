using System;

namespace Magnesium
{
	public enum MgSamplerYcbcrModelConversion : UInt32
	{
		RGB_IDENTITY = 0,
		/// <summary> 
		/// just range expansion
		/// </summary> 
		YCBCR_IDENTITY = 1,
		/// <summary> 
		/// aka HD YUV
		/// </summary> 
		YCBCR_709 = 2,
		/// <summary> 
		/// aka SD YUV
		/// </summary> 
		YCBCR_601 = 3,
		/// <summary> 
		/// aka UHD YUV
		/// </summary> 
		YCBCR_2020 = 4,
		RGB_IDENTITY_KHR = RGB_IDENTITY,
		YCBCR_IDENTITY_KHR = YCBCR_IDENTITY,
		YCBCR_709_KHR = YCBCR_709,
		YCBCR_601_KHR = YCBCR_601,
		YCBCR_2020_KHR = YCBCR_2020,
	}
}
