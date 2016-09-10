using System.Runtime.InteropServices;
using System;
using Magnesium;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLGraphicsPipelineBlendColorAttachmentState : IEquatable<GLGraphicsPipelineBlendColorAttachmentState>
	{
		public bool BlendEnable { get; set; }
		public MgBlendFactor SrcColorBlendFactor {get;set;}
		public MgBlendFactor DstColorBlendFactor {get;set;}
		public MgBlendOp ColorBlendOp {get;set;}

		public MgBlendFactor SrcAlphaBlendFactor {get;set;}
		public MgBlendFactor DstAlphaBlendFactor {get;set;}
		public MgBlendOp AlphaBlendOp {get;set;}
		public MgColorComponentFlagBits ColorWriteMask { get; set; }

		#region IEquatable implementation
		public bool Equals (GLGraphicsPipelineBlendColorAttachmentState other)
		{
			return 
				this.BlendEnable == other.BlendEnable
				&& this.ColorBlendOp == other.ColorBlendOp
				&& this.AlphaBlendOp == other.AlphaBlendOp
				&& this.SrcColorBlendFactor == other.SrcColorBlendFactor
				&& this.DstColorBlendFactor == other.DstColorBlendFactor
				&& this.SrcAlphaBlendFactor == other.SrcAlphaBlendFactor
				&& this.DstAlphaBlendFactor == other.DstAlphaBlendFactor
				&& this.ColorWriteMask == other.ColorWriteMask;
		}
		#endregion

	}
}

