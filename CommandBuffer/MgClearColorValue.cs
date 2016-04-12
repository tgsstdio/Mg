using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Explicit)]
	public struct MgClearColorValue
	{
		[FieldOffset(0)]
		public MgVec4f Float32; // m4;
		[FieldOffset(0)]
		public MgVec4i Int32; // m4;
		[FieldOffset(0)]
		public MgVec4Ui Uint32; // m4i	
	}
}

