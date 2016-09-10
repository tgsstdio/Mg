using System.Runtime.InteropServices;
using System;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]
	public struct MgColor4f : IEquatable<MgColor4f>
	{
		public MgColor4f (float r, float g, float b, float a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public float R { get; set; }
		public float G { get; set; }
		public float B { get; set; }
		public float A { get; set; }

		#region IEquatable implementation

		public bool Equals (MgColor4f other)
		{
			return Math.Abs (this.R - other.R) <= float.Epsilon
				&& Math.Abs (this.G - other.G) <= float.Epsilon
				&& Math.Abs (this.B - other.B) <= float.Epsilon
				&& Math.Abs (this.A - other.A) <= float.Epsilon;
		}

		#endregion
	}
}

