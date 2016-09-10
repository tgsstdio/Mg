using System.Runtime.InteropServices;
using System;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLCmdBufferPipelineItem  : IEquatable<GLCmdBufferPipelineItem>
	{
		public GLGraphicsPipelineFlagBits Flags {get;set;}

		public GLGraphicsPipelineDepthState DepthState { get; set;	}

		public GLGraphicsPipelineStencilState StencilState { get; set; }

//		public byte Pass { get; set; }
//
//		public byte Pipeline { get; set; }

		public byte ColorBlendEnums { get; set; }

		public byte ClearValues { get; set; }

		#region IEquatable implementation
		public bool Equals (GLCmdBufferPipelineItem other)
		{
//			if (!Pipeline.Equals (other.Pipeline))
//				return false;
//
//			if (!Pass.Equals (other.Pass))
//				return false;

			if (!Flags.Equals (other.Flags))
				return false;

			if (!DepthState.Equals (other.DepthState))
				return false;

			if (!StencilState.Equals (other.StencilState))
				return false;

//			if (!Rasterization.Equals (other.Rasterization))
//				return false;

			if (!ColorBlendEnums.Equals (other.ColorBlendEnums))
				return false;

			if (!ClearValues.Equals (other.ClearValues))
				return false;

			return true;
		}
		#endregion
	}
}

