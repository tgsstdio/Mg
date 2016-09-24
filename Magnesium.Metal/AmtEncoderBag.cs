using System;
using System.Collections.Generic;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDepthBiasParameter
	{
		public float DepthBias;
		public float SlopeScale;
		public float Clamp;
	}

	public struct AmtStencilReferenceMask
	{
		public uint Front;
		public uint Back;
	}

	public class AmtCmdInstruction
	{
		public uint Index { get;  set;}
		public Action<IMTLRenderCommandEncoder, AmtEncoderItemGrid, uint> Setter { get;  set; }
	}

	public class AmtItemBag<TData>
	{
		public AmtItemBag()
		{
			mItems = new List<TData>();
		}
		private readonly List<TData> mItems;

		public uint Push(TData item)
		{
			var count = (uint)mItems.Count;
			mItems.Add(item);
			return count;
		}

		public void Clear()
		{
			mItems.Clear();
		}

		public TData[] ToArray()
		{
			return mItems.ToArray();
		}
	}

	public class AmtEncoderItemBag
	{
		public AmtEncoderItemBag()
		{
			mBlendConstants = new AmtItemBag<MgColor4f>();
			mPipelineStates = new AmtItemBag<IMTLRenderPipelineState>();
			mDepthStencilStates = new AmtItemBag<IMTLDepthStencilState>();
			mDepthBias = new AmtItemBag<AmtDepthBiasParameter>();
			mStencilReferences = new AmtItemBag<AmtStencilReferenceMask>();
			mInstructions = new List<AmtCmdInstruction>();
			mViewports = new AmtItemBag<MTLViewport>();
		}

		public void Clear()
		{
			mBlendConstants.Clear();
			mPipelineStates.Clear();
			mDepthStencilStates.Clear();
			mDepthBias.Clear();
			mStencilReferences.Clear();
			mViewports.Clear();
			mInstructions.Clear();
		}

		private readonly AmtItemBag<MTLViewport> mViewports;
		public AmtItemBag<MTLViewport> Viewports
		{
			get
			{
				return mViewports;
			}
		}

		private readonly List<AmtCmdInstruction> mInstructions;
		public List<AmtCmdInstruction> Instructions
		{
			get
			{
				return mInstructions;
			}
		}

		private readonly AmtItemBag<MgColor4f> mBlendConstants;
		public AmtItemBag<MgColor4f> BlendConstants
		{
			get
			{
				return mBlendConstants;
			}
		}

		private readonly AmtItemBag<IMTLRenderPipelineState> mPipelineStates;
		public AmtItemBag<IMTLRenderPipelineState> PipelineStates
		{
			get
			{
				return mPipelineStates;
			}

		}

		private readonly AmtItemBag<IMTLDepthStencilState> mDepthStencilStates;
		public AmtItemBag<IMTLDepthStencilState> DepthStencilStates
		{
			get
			{
				return mDepthStencilStates;
			}
		}

		private readonly AmtItemBag<AmtDepthBiasParameter> mDepthBias;
		public AmtItemBag<AmtDepthBiasParameter> DepthBias 
		{ 
			get
			{
				return mDepthBias;
			}
		}

		private readonly AmtItemBag<AmtStencilReferenceMask> mStencilReferences;
		public AmtItemBag<AmtStencilReferenceMask> StencilReferences
		{
			get
			{
				return mStencilReferences;
			}
		}
	}

	public class AmtCmdGraphicsPipelineDetail
	{
		public IMTLRenderPipelineState PipelineState { get; internal set; }
		public IMTLDepthStencilState DepthStencilState { get; internal set; }
		public uint FrontReference { get; internal set; }
		public uint BackReference { get; internal set; }
		public MTLCullMode CullMode { get; internal set;}
		public MTLWinding Winding { get; internal set; }
		public MTLTriangleFillMode FillMode { get; internal set; }
		public MTLViewport Viewport { get; internal set; }
		public MTLScissorRect Scissor { get; internal set; }
	}

	public class AmtEncoderItemGrid
	{
		public AmtCmdInstruction[] Instructions { get; internal set; }

		public MgColor4f[] BlendConstants { get; internal set; }
		public AmtCmdGraphicsPipelineDetail[] PipelineDetails { get; internal set;}
		public IMTLDepthStencilState[] DepthStencilStates { get; internal set;}
		public AmtDepthBiasParameter[] DepthBias { get; internal set; }
		public AmtStencilReferenceMask[] StencilReferences { get; internal set;}
		public MTLViewport[] Viewports { get; internal set;}
	}
}
