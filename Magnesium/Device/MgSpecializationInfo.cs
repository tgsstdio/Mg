using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    public class MgSpecializationInfo
	{
		public MgSpecializationMapEntry[] MapEntries { get; set; }
		public UIntPtr DataSize { get; set; }
		public IntPtr Data { get; set; }
	}
}

