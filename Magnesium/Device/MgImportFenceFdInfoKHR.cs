using System;

namespace Magnesium
{
	public class MgImportFenceFdInfoKHR
	{
		public IMgFence Fence { get; set; }
		public MgFenceImportFlagBits Flags { get; set; }
		public MgExternalFenceHandleTypeFlagBits HandleType { get; set; }
		public int Fd { get; set; }
	}
}
