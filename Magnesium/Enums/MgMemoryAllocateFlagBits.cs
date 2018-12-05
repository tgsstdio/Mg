using System;

namespace Magnesium
{
	[Flags]
	public enum MgMemoryAllocateFlagBits : UInt32
	{
		/// <summary> 
		/// Force allocation on specific devices
		/// </summary> 
		DEVICE_MASK_BIT = 0x1,
		DEVICE_MASK_BIT_KHR = DEVICE_MASK_BIT,
	}
}
