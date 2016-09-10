using System.Runtime.InteropServices;
using System;
using Magnesium;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLGraphicsPipelineStencilState : IEquatable<GLGraphicsPipelineStencilState>, IComparable<GLGraphicsPipelineStencilState>
	{
		public MgCompareOp FrontStencilFunction { get; set; }
		public MgStencilOp FrontStencilPass { get; set; }
		public MgStencilOp FrontStencilFail {get;set;}
		public MgStencilOp FrontDepthBufferFail { get; set; }

		public MgCompareOp BackStencilFunction { get; set; }
		public MgStencilOp BackStencilPass { get; set; }
		public MgStencilOp BackStencilFail {get;set;}
		public MgStencilOp BackDepthBufferFail { get; set; }

		//	public DepthStencilBitFlags Flags { get; set; }


		#region IEquatable implementation

		public bool Equals (GLGraphicsPipelineStencilState other)
		{
			return 
				this.FrontStencilFunction == other.FrontStencilFunction
				//&& this.DepthBufferFunction == other.DepthBufferFunction
			&& this.FrontStencilPass == other.FrontStencilPass
			&& this.FrontStencilFail == other.FrontStencilFail
			&& this.FrontDepthBufferFail == other.FrontDepthBufferFail
			&& this.BackStencilFunction == other.BackStencilFunction
			&& this.BackStencilPass == other.BackStencilPass
			&& this.BackStencilFail == other.BackStencilFail
			&& this.BackDepthBufferFail == other.BackDepthBufferFail;
		}

		#endregion

		#region IComparable implementation

		public int CompareTo (GLGraphicsPipelineStencilState other)
		{
			if (FrontStencilFunction < other.FrontStencilFunction)
			{
				return -1;
			}
			else if (FrontStencilFunction > other.FrontStencilFunction)
			{
				return 1;
			}

			if (FrontStencilPass < other.FrontStencilPass)
			{
				return -1;
			}
			else if (FrontStencilPass > other.FrontStencilPass)
			{
				return 1;
			}

			if (FrontStencilFail < other.FrontStencilFail)
			{
				return -1;
			}
			else if (FrontStencilFail > other.FrontStencilFail)
			{
				return 1;
			}

			if (FrontDepthBufferFail < other.FrontDepthBufferFail)
			{
				return -1;
			}
			else if (FrontDepthBufferFail > other.FrontDepthBufferFail)
			{
				return 1;
			}

			if (BackStencilFunction < other.BackStencilFunction)
			{
				return -1;
			}
			else if (BackStencilFunction > other.BackStencilFunction)
			{
				return 1;
			}

			if (BackStencilPass < other.BackStencilPass)
			{
				return -1;
			}
			else if (BackStencilPass > other.BackStencilPass)
			{
				return 1;
			}

			if (BackStencilFail < other.BackStencilFail)
			{
				return -1;
			}
			else if (BackStencilFail > other.BackStencilFail)
			{
				return 1;
			}

			if (BackDepthBufferFail < other.BackDepthBufferFail)
			{
				return -1;
			}
			else if (BackDepthBufferFail > other.BackDepthBufferFail)
			{
				return 1;
			}

			return 0;
		}

		#endregion
	}
}

