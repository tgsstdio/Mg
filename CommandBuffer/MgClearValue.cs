using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Explicit)]
	public struct MgClearValue
	{
		[FieldOffset(0)]
		public MgClearColorValue Color;
		[FieldOffset(0)]
		public MgClearDepthStencilValue DepthStencil;
	}
}

