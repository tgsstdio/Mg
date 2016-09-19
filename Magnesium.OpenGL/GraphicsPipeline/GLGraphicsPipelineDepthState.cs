using System;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLGraphicsPipelineDepthState : IEquatable<GLGraphicsPipelineDepthState>, IComparable<GLGraphicsPipelineDepthState>
	{
		public MgCompareOp DepthBufferFunction { get; set; }
		//	public ClipControl DepthRange { get; set; }

		#region IEquatable implementation

		public bool Equals (GLGraphicsPipelineDepthState other)
		{
			return	this.DepthBufferFunction == other.DepthBufferFunction;
		}

		#endregion

		#region IComparable implementation

		public int CompareTo (GLGraphicsPipelineDepthState other)
		{
			if (DepthBufferFunction < other.DepthBufferFunction)
			{
				return -1;
			}
			else if (DepthBufferFunction > other.DepthBufferFunction)
			{
				return 1;
			}
			else
			{
				return 0;
			}				
		}

		#endregion
	}
}

