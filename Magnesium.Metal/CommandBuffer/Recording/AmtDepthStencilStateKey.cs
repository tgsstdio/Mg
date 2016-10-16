using System;
using Metal;

namespace Magnesium.Metal
{
	public struct AmtDepthStencilStateKey :
		IEquatable<AmtDepthStencilStateKey>,
		IComparable<AmtDepthStencilStateKey>
	{
		public MgCompareOp DepthCompareFunction { get; set; }
		public bool DepthWriteEnabled { get; set;}
		public AmtGraphicsPipelineStencilInfo Front { get; set; }
		public AmtGraphicsPipelineStencilInfo Back { get; set; }

		public int CompareTo(AmtDepthStencilStateKey other)
		{
			if (DepthCompareFunction < other.DepthCompareFunction)
				return -1;

			if (DepthCompareFunction > other.DepthCompareFunction)
				return 1;

			var frontValue = Front.CompareTo(other.Front);
			if (frontValue < 0)
				return -1;

			if (frontValue > 0)
				return 1;

			var backValue = Back.CompareTo(other.Back);
			if (backValue < 0)
				return -1;

			if (backValue > 0)
				return 1;                    


			if (!other.DepthWriteEnabled && DepthWriteEnabled)
			{
				return -1;
			}
			else if (other.DepthWriteEnabled && !DepthWriteEnabled)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}

		public bool Equals(AmtDepthStencilStateKey other)
		{
			return
					(this.DepthCompareFunction == other.DepthCompareFunction)
				&& (this.DepthWriteEnabled == other.DepthWriteEnabled)
				&& (this.Front.Equals(other.Front))
				&& (this.Back.Equals(other.Back));
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				// Suitable nullity checks etc, of course :)
				hash = hash * 23 + DepthCompareFunction.GetHashCode();
				hash = hash * 23 + DepthWriteEnabled.GetHashCode();
				hash = hash * 23 + Front.GetHashCode();
				hash = hash * 23 + Back.GetHashCode();
				return hash;
			}
		}

		private static MTLCompareFunction GetDepthCompareFunction(MgCompareOp depthCompareOp)
		{
			switch (depthCompareOp)
			{
				default:
					throw new NotSupportedException();
				case MgCompareOp.LESS:
					return MTLCompareFunction.Less;
				case MgCompareOp.LESS_OR_EQUAL:
					return MTLCompareFunction.LessEqual;
				case MgCompareOp.GREATER:
					return MTLCompareFunction.Greater;
				case MgCompareOp.GREATER_OR_EQUAL:
					return MTLCompareFunction.GreaterEqual;
				case MgCompareOp.NEVER:
					return MTLCompareFunction.Never;
				case MgCompareOp.NOT_EQUAL:
					return MTLCompareFunction.NotEqual;
				case MgCompareOp.EQUAL:
					return MTLCompareFunction.Equal;
			}
		}

		public MTLDepthStencilDescriptor GenerateDescriptor()
		{
			return new MTLDepthStencilDescriptor
			{
				// TODO : add this back in later
				DepthCompareFunction = GetDepthCompareFunction(DepthCompareFunction),
				DepthWriteEnabled = DepthWriteEnabled,
				BackFaceStencil = Back.GetDescriptor(),
				FrontFaceStencil = Front.GetDescriptor(),
			};

			//dsDescriptor.FrontFaceStencil.ReadMask = frontCompareMask;
			//dsDescriptor.FrontFaceStencil.WriteMask = frontWriteMask;
			//dsDescriptor.BackFaceStencil.ReadMask = backCompareMask;
			//dsDescriptor.BackFaceStencil.WriteMask = backWriteMask;
		}
	}
}