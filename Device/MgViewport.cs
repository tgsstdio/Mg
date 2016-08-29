using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgViewport
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public float MinDepth { get; set; }
		public float MaxDepth { get; set; }
	}
}

