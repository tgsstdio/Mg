using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]	
	public struct MgRect2D
	{
		public MgOffset2D Offset { get; set; }
		public MgExtent2D Extent { get; set; }
	}
}

