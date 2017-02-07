using System;

namespace Magnesium.OpenGL
{
	public struct GLClearValueState : IEquatable<GLClearValueState>
	{
		public MgColor4f ClearColor { get; set; }
		public float DepthValue { get; set; }
		public uint StencilValue { get; set; }

		#region IEquatable implementation

		public bool Equals (GLClearValueState other)
		{
			if (StencilValue != other.StencilValue)
			{
				return false;
			}

			if (Math.Abs (DepthValue - other.DepthValue) > float.Epsilon)
			{
				return false;
			}

			return ClearColor.Equals (other.ClearColor);
		}

		#endregion
	}
}

