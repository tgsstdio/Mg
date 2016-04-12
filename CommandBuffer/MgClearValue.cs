using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Explicit)]
	public struct MgClearValue
	{
		[FieldOffset(0)]
		MgClearColorValue Color; 
		[FieldOffset(0)]
		MgClearDepthStencilValue DepthStencil; 
	}
}

