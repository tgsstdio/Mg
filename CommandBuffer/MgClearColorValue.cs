using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Explicit)]
	public struct MgClearColorValue
	{
		public MgClearColorValue (MgColor4f color)
		{
			Int32 = new MgVec4i (0, 0, 0, 0);
			Uint32 = new MgVec4Ui (0, 0, 0, 0);

			Float32 = color;
		}

		public MgClearColorValue(MgVec4i color)
		{
			Uint32 = new MgVec4Ui (0, 0, 0, 0);
			Float32 = new MgColor4f (0f, 0f, 0f, 0f);

			Int32 = color;
		}

		public MgClearColorValue(MgVec4Ui color)
		{
			Int32 = new MgVec4i (0, 0, 0, 0);
			Float32 = new MgColor4f (0f, 0f, 0f, 0f);

			Uint32 = color;
		}

		[FieldOffset(0)]
		public MgColor4f Float32; // m4;
		[FieldOffset(0)]
		public MgVec4i Int32; // m4;
		[FieldOffset(0)]
		public MgVec4Ui Uint32; // m4i	
	}
}

