using System;

namespace Magnesium
{
	public class MgPhysicalDeviceExternalBufferInfo
	{
		public MgBufferCreateFlagBits Flags { get; set; }
		public MgBufferUsageFlagBits Usage { get; set; }
		public MgExternalMemoryHandleTypeFlagBits HandleType { get; set; }
	}
}
