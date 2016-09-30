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
			mDepthStencilStates = new AmtEncoderItemCollection<AmtDepthStencilStateEncoderState>();
			mDepthBias = new AmtEncoderItemCollection<AmtDepthBiasEncoderState>();
			mStencilReferences = new AmtEncoderItemCollection<AmtStencilReferenceEncoderState>();
			mRenderPasses = new AmtEncoderItemCollection<MTLRenderPassDescriptor>();
			mScissors = new AmtEncoderItemCollection<MTLScissorRect>();
			mViewports = new AmtEncoderItemCollection<MTLViewport>();
			mDraws = new AmtEncoderItemCollection<AmtDrawEncoderState>();
			mDrawIndirects = new AmtEncoderItemCollection<AmtDrawIndirectEncoderState>();
			mDrawIndexeds = new AmtEncoderItemCollection<AmtDrawIndexedEncoderState>();
			mDrawIndexedIndirects = new AmtEncoderItemCollection<AmtDrawIndexedIndirectEncoderState>();
		}

		public void Clear()
		{
			mBlendConstants.Clear();
			mPipelineStates.Clear();
			mDepthStencilStates.Clear();
			mDepthBias.Clear();
			mStencilReferences.Clear();
			mScissors.Clear();
			mViewports.Clear();
			mRenderPasses.Clear();
			mDraws.Clear();
			mDrawIndirects.Clear();
			mDrawIndexeds.Clear();
			mDrawIndexedIndirects.Clear();
		}

		private AmtEncoderItemCollection<MTLScissorRect> mScissors;
		public AmtEncoderItemCollection<MTLScissorRect> Scissors
		{
			get
			{
				return mScissors;
			}
		}

		private AmtEncoderItemCollection<AmtDrawIndirectEncoderState> mDrawIndirects;
		public AmtEncoderItemCollection<AmtDrawIndirectEncoderState> DrawIndirects
		{
			get
			{
				return mDrawIndirects;
			}
		}

		private AmtEncoderItemCollection<AmtDrawEncoderState> mDraws;
		public AmtEncoderItemCollection<AmtDrawEncoderState> Draws
		{
			get
			{
				return mDraws;
			}
		}

		private AmtEncoderItemCollection<AmtDrawIndexedIndirectEncoderState> mDrawIndexedIndirects;
		public AmtEncoderItemCollection<AmtDrawIndexedIndirectEncoderState> DrawIndexedIndirects
		{
			get
			{
				return mDrawIndexedIndirects;
			}
		}

		private AmtEncoderItemCollection<AmtDrawIndexedEncoderState> mDrawIndexeds;
		public AmtEncoderItemCollection<AmtDrawIndexedEncoderState> DrawIndexeds
		{
			get
			{
				return mDrawIndexeds;
			}
		}

		private readonly AmtEncoderItemCollection<MTLViewport> mViewports;
		public AmtEncoderItemCollection<MTLViewport> Viewports
		{
			get
			{
				return mViewports;
			}
		}

		private readonly AmtEncoderItemCollection<MTLRenderPassDescriptor> mRenderPasses;
		public AmtEncoderItemCollection<MTLRenderPassDescriptor> RenderPasses
		{
			get
			{
				return mRenderPasses;
			}
		}

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

		private readonly AmtEncoderItemCollection<AmtDepthStencilStateEncoderState> mDepthStencilStates;
		public AmtEncoderItemCollection<AmtDepthStencilStateEncoderState> DepthStencilStates
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
