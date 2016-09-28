using System;
using System.Collections.Generic;
using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsEncoderItemBag
	{
		public AmtGraphicsEncoderItemBag()
		{
			mBlendConstants = new AmtEncoderItemCollection<MgColor4f>();
			mPipelineStates = new AmtEncoderItemCollection<AmtPipelineEncoderState>();
			mDepthStencilStates = new AmtEncoderItemCollection<IMTLDepthStencilState>();
			mDepthBias = new AmtEncoderItemCollection<AmtDepthBiasEncoderState>();
			mStencilReferences = new AmtEncoderItemCollection<AmtStencilReferenceEncoderState>();
			//mInstructions = new List<AmtCommandEncoderInstruction>();
			mViewports = new AmtEncoderItemCollection<MTLViewport>();
		}

		public void Clear()
		{
			mBlendConstants.Clear();
			mPipelineStates.Clear();
			mDepthStencilStates.Clear();
			mDepthBias.Clear();
			mStencilReferences.Clear();
			mViewports.Clear();
			//mInstructions.Clear();
		}

		private readonly AmtEncoderItemCollection<MTLViewport> mViewports;
		public AmtEncoderItemCollection<MTLViewport> Viewports
		{
			get
			{
				return mViewports;
			}
		}

		//private readonly List<AmtCommandEncoderInstruction> mInstructions;
		//public List<AmtCommandEncoderInstruction> Instructions
		//{
		//	get
		//	{
		//		return mInstructions;
		//	}
		//}

		private readonly AmtEncoderItemCollection<MgColor4f> mBlendConstants;
		public AmtEncoderItemCollection<MgColor4f> BlendConstants
		{
			get
			{
				return mBlendConstants;
			}
		}

		private readonly AmtEncoderItemCollection<AmtPipelineEncoderState> mPipelineStates;
		public AmtEncoderItemCollection<AmtPipelineEncoderState> PipelineStates
		{
			get
			{
				return mPipelineStates;
			}

		}

		private readonly AmtEncoderItemCollection<IMTLDepthStencilState> mDepthStencilStates;
		public AmtEncoderItemCollection<IMTLDepthStencilState> DepthStencilStates
		{
			get
			{
				return mDepthStencilStates;
			}
		}

		private readonly AmtEncoderItemCollection<AmtDepthBiasEncoderState> mDepthBias;
		public AmtEncoderItemCollection<AmtDepthBiasEncoderState> DepthBias
		{
			get
			{
				return mDepthBias;
			}
		}

		private readonly AmtEncoderItemCollection<AmtStencilReferenceEncoderState> mStencilReferences;
		public AmtEncoderItemCollection<AmtStencilReferenceEncoderState> StencilReferences
		{
			get
			{
				return mStencilReferences;
			}
		}
	}
}
