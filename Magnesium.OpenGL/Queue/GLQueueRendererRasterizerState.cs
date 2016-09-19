using System;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLQueueRendererRasterizerState : IEquatable<GLQueueRendererRasterizerState>
	{
		public GLGraphicsPipelineFlagBits Flags { get; set; }
		public GLCmdDepthBiasParameter DepthBias { get; set; }
		public float LineWidth { get; set; }

		#region IEquatable implementation

		public bool Equals (GLQueueRendererRasterizerState other)
		{
			return ((Flags & other.Flags) == Flags)
				&& Math.Abs (this.DepthBias.DepthBiasConstantFactor - other.DepthBias.DepthBiasConstantFactor) <= float.Epsilon
				&& Math.Abs (this.DepthBias.DepthBiasSlopeFactor - other.DepthBias.DepthBiasSlopeFactor) <= float.Epsilon
				&& Math.Abs (this.DepthBias.DepthBiasClamp - other.DepthBias.DepthBiasClamp) <= float.Epsilon
				&& Math.Abs (this.LineWidth - other.LineWidth) <= float.Epsilon;
		}

		#endregion
	}
}

