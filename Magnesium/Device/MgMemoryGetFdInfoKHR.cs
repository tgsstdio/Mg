using System;

namespace Magnesium
{
	public class MgMemoryGetFdInfoKHR
	{
		public IMgDeviceMemory Memory { get; set; }
		public MgExternalMemoryHandleTypeFlagBits HandleType { get; set; }
	}
}
