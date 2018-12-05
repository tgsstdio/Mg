using System;

namespace Magnesium
{
	public enum MgSamplerYcbcrRange : UInt32
	{
		/// <summary> 
		/// Luma 0..1 maps to 0..255, chroma -0.5..0.5 to 1..255 (clamped)
		/// </summary> 
		ITU_FULL = 0,
		/// <summary> 
		/// Luma 0..1 maps to 16..235, chroma -0.5..0.5 to 16..240
		/// </summary> 
		ITU_NARROW = 1,
		ITU_FULL_KHR = ITU_FULL,
		ITU_NARROW_KHR = ITU_NARROW,
	}
}
