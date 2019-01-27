using System;

namespace Magnesium
{
	public class MgBindImageMemoryInfo
	{
		public IMgImage Image { get; set; }
		public IMgDeviceMemory Memory { get; set; }
		public UInt64 MemoryOffset { get; set; }
	}
}
