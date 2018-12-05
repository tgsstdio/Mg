using System;

namespace Magnesium
{
	[Flags]
	public enum MgDeviceGroupPresentModeFlagBitsKHR : UInt32
	{
		/// <summary> 
		/// Present from local memory
		/// </summary> 
		LOCAL_BIT_KHR = 0x1,
		/// <summary> 
		/// Present from remote memory
		/// </summary> 
		REMOTE_BIT_KHR = 0x2,
		/// <summary> 
		/// Present sum of local and/or remote memory
		/// </summary> 
		SUM_BIT_KHR = 0x4,
		/// <summary> 
		/// Each physical device presents from local memory
		/// </summary> 
		LOCAL_MULTI_DEVICE_BIT_KHR = 0x8,
	}
}
