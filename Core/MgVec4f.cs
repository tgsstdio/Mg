using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]
	public struct MgVec4f
	{
		public float X {get;set;}
		public float Y {get;set;}
		public float Z {get;set;}
		public float W {get;set;}
	}
}

