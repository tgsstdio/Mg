using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]	
	public struct MgComponentMapping
	{
		public MgComponentSwizzle R { get; set; }
		public MgComponentSwizzle G { get; set; }
		public MgComponentSwizzle B { get; set; }
		public MgComponentSwizzle A { get; set; }
	}
}

