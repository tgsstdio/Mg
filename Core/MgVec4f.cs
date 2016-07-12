using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]
	public struct MgVec4f
	{
		public MgVec4f (float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public float X {get;set;}
		public float Y {get;set;}
		public float Z {get;set;}
		public float W {get;set;}
	}
}

