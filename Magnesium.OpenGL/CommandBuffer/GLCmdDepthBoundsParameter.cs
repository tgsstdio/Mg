using System;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct GLCmdDepthBoundsParameter  : IEquatable<GLCmdDepthBoundsParameter>
	{
		public float MinDepthBounds { get; set; }
		public float MaxDepthBounds { get; set; }

		#region IEquatable implementation

		public bool Equals (GLCmdDepthBoundsParameter other)
		{
			return Math.Abs (this.MinDepthBounds - other.MinDepthBounds) <= float.Epsilon
				&& Math.Abs (this.MaxDepthBounds - other.MaxDepthBounds) <= float.Epsilon;
		}

		#endregion
	}

}

