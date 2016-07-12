using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Explicit)]
	public struct MgClearColorValue
	{
		public MgClearColorValue (float r, float g, float b, float a)
		{
			Int32 = new MgVec4i (0, 0, 0, 0);
			Uint32 = new MgVec4Ui (0, 0, 0, 0);

			Float32 = new MgVec4f (r, g, b, a);
		}

		public MgClearColorValue(int r, int g, int b, int a)
		{
			Uint32 = new MgVec4Ui (0, 0, 0, 0);
			Float32 = new MgVec4f (0f, 0f, 0f, 0f);

			Int32 = new MgVec4i (r, g, b, a);
		}

		public MgClearColorValue(uint r, uint g, uint b, uint a)
		{
			Int32 = new MgVec4i (0, 0, 0, 0);
			Float32 = new MgVec4f (0f, 0f, 0f, 0f);

			Uint32 = new MgVec4Ui (r, g, b, a);
		}

		[FieldOffset(0)]
		public MgVec4f Float32; // m4;
		[FieldOffset(0)]
		public MgVec4i Int32; // m4;
		[FieldOffset(0)]
		public MgVec4Ui Uint32; // m4i	
	}
}

