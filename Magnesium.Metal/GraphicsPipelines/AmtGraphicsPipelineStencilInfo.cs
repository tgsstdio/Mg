using System;
using Metal;

namespace Magnesium.Metal
{
	public struct AmtGraphicsPipelineStencilInfo :
		IEquatable<AmtGraphicsPipelineStencilInfo>,
		IComparable<AmtGraphicsPipelineStencilInfo>
	{
		public MgCompareOp StencilCompareFunction { get; internal set; }
		public MgStencilOp DepthFailure { get; internal set; }
		public MgStencilOp DepthStencilPass { get; internal set; }
		public uint ReadMask { get; internal set; }
		public MgStencilOp StencilFailure { get; internal set; }
		public uint WriteMask { get; internal set; }
		//public uint StencilReference { get; internal set; }

		public MTLStencilDescriptor GetDescriptor()
		{
			return new MTLStencilDescriptor
			{
				DepthFailureOperation = GetStencilOperation(DepthFailure),
				DepthStencilPassOperation = GetStencilOperation(DepthStencilPass),
				ReadMask = ReadMask,
				WriteMask = WriteMask,
				StencilCompareFunction = GetCompareFunction(StencilCompareFunction),
				StencilFailureOperation = GetStencilOperation(StencilFailure),
			};
		}

		public bool Equals(AmtGraphicsPipelineStencilInfo other)
		{
			return (StencilCompareFunction == other.StencilCompareFunction)
				&& (DepthFailure == other.DepthFailure)
				&& (DepthStencilPass == other.DepthStencilPass)
				&& (ReadMask == other.ReadMask)
				&& (StencilFailure == other.StencilFailure)
				&& (WriteMask == other.WriteMask);
				//&& (StencilReference == other.StencilReference);
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				hash = hash * 23 + StencilCompareFunction.GetHashCode();
				hash = hash * 23 + DepthFailure.GetHashCode();
				hash = hash * 23 + DepthStencilPass.GetHashCode();
				hash = hash * 23 + ReadMask.GetHashCode();
				hash = hash * 23 + StencilFailure.GetHashCode();
				hash = hash * 23 + WriteMask.GetHashCode();
				//hash = hash * 23 + StencilReference.GetHashCode();
				return hash;
			}
		}

		public int CompareTo(AmtGraphicsPipelineStencilInfo other)
		{
			// StencilCompareFunction
			if (StencilCompareFunction < other.StencilCompareFunction)
				return -1;

			if (StencilCompareFunction > other.StencilCompareFunction)
				return 1;

			//DepthFailure
			if (DepthFailure < other.DepthFailure)
				return -1;

			if (DepthFailure > other.DepthFailure)
				return 1;

			// DepthStencilPass
			if (DepthStencilPass < other.DepthStencilPass)
				return -1;

			if (DepthStencilPass > other.DepthStencilPass)
				return -1;

			// ReadMask
			if (ReadMask < other.ReadMask)
				return -1;

			if (ReadMask > other.ReadMask)
				return 1;

			// StencilFailure
			if (StencilFailure < other.StencilFailure)
				return -1;

			if (StencilFailure > other.StencilFailure)
				return 1;

			// WriteMask
			if (WriteMask < other.WriteMask)
				return -1;

			if (WriteMask > other.WriteMask)
				return 1;

			// StencilReference
			//if (StencilReference < other.StencilReference)
			//	return -1;

			//if (StencilReference > other.StencilReference)
			//	return 1;

			return 0;
		}

		public static MTLStencilOperation GetStencilOperation(MgStencilOp stencilOp)
		{
			switch (stencilOp)
			{
				default:
					throw new NotSupportedException();
				case MgStencilOp.DECREMENT_AND_CLAMP:
					return MTLStencilOperation.DecrementClamp;
				case MgStencilOp.DECREMENT_AND_WRAP:
					return MTLStencilOperation.DecrementWrap;
				case MgStencilOp.INCREMENT_AND_CLAMP:
					return MTLStencilOperation.IncrementClamp;
				case MgStencilOp.INCREMENT_AND_WRAP:
					return MTLStencilOperation.IncrementWrap;
				case MgStencilOp.INVERT:
					return MTLStencilOperation.Invert;
				case MgStencilOp.KEEP:
					return MTLStencilOperation.Keep;
				case MgStencilOp.REPLACE:
					return MTLStencilOperation.Replace;
				case MgStencilOp.ZERO:
					return MTLStencilOperation.Zero;
			}
		}

		public static MTLCompareFunction GetCompareFunction(MgCompareOp depthCompareOp)
		{
			switch (depthCompareOp)
			{
				default:
					throw new NotSupportedException();
				case MgCompareOp.ALWAYS:
					return MTLCompareFunction.Always;
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
	}
}