using System;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential, Size = 12)]
	public struct GLCmdDepthBiasParameter : IEquatable<GLCmdDepthBiasParameter>
	{
		public float DepthBiasConstantFactor { get; set; }
		public float DepthBiasClamp { get; set; }
		public float DepthBiasSlopeFactor { get; set; }

		#region IEquatable implementation

		public bool Equals (GLCmdDepthBiasParameter other)
		{
			return Math.Abs (this.DepthBiasConstantFactor - other.DepthBiasConstantFactor) <= float.Epsilon
				&& Math.Abs (this.DepthBiasClamp - other.DepthBiasClamp) <= float.Epsilon		
				&& Math.Abs (this.DepthBiasSlopeFactor - other.DepthBiasSlopeFactor) <= float.Epsilon;
		}

		#endregion
	}
}

