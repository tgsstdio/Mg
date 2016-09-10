using System;

namespace Magnesium
{
    public class MgPipelineCacheCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UIntPtr InitialDataSize { get; set; }
		public IntPtr InitialData { get; set; }
	}
}

