using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsBag
	{
		public AmtGraphicsBag()
		{
			mBlendConstants = new AmtEncoderItemCollection<MgColor4f>();
			mPipelineStates = new AmtEncoderItemCollection<AmtPipelineStateRecord>();
			mDepthStencilStates = new AmtEncoderItemCollection<IMTLDepthStencilState>();
			mDepthBias = new AmtEncoderItemCollection<AmtDepthBiasRecord>();
			mStencilReferences = new AmtEncoderItemCollection<AmtStencilReferenceRecord>();
			mRenderPasses = new AmtEncoderItemCollection<MTLRenderPassDescriptor>();
			mScissors = new AmtEncoderItemCollection<MTLScissorRect>();
			mViewports = new AmtEncoderItemCollection<MTLViewport>();
			mDraws = new AmtEncoderItemCollection<AmtDrawRecord>();
			mDrawIndirects = new AmtEncoderItemCollection<AmtDrawIndirectRecord>();
			mDrawIndexeds = new AmtEncoderItemCollection<AmtDrawIndexedRecord>();
			mDrawIndexedIndirects = new AmtEncoderItemCollection<AmtDrawIndexedIndirectRecord>();
			mVertexBuffers = new AmtEncoderItemCollection<AmtVertexBufferRecord>();
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
			mVertexBuffers.Clear();
		}

		private AmtEncoderItemCollection<AmtVertexBufferRecord> mVertexBuffers;
		public AmtEncoderItemCollection<AmtVertexBufferRecord> VertexBuffers
		{
			get
			{
				return mVertexBuffers;
			}
		}

		private AmtEncoderItemCollection<MTLScissorRect> mScissors;
		public AmtEncoderItemCollection<MTLScissorRect> Scissors
		{
			get
			{
				return mScissors;
			}
		}

		private AmtEncoderItemCollection<AmtDrawIndirectRecord> mDrawIndirects;
		public AmtEncoderItemCollection<AmtDrawIndirectRecord> DrawIndirects
		{
			get
			{
				return mDrawIndirects;
			}
		}

		private AmtEncoderItemCollection<AmtDrawRecord> mDraws;
		public AmtEncoderItemCollection<AmtDrawRecord> Draws
		{
			get
			{
				return mDraws;
			}
		}

		private AmtEncoderItemCollection<AmtDrawIndexedIndirectRecord> mDrawIndexedIndirects;
		public AmtEncoderItemCollection<AmtDrawIndexedIndirectRecord> DrawIndexedIndirects
		{
			get
			{
				return mDrawIndexedIndirects;
			}
		}

		private AmtEncoderItemCollection<AmtDrawIndexedRecord> mDrawIndexeds;
		public AmtEncoderItemCollection<AmtDrawIndexedRecord> DrawIndexeds
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

		private readonly AmtEncoderItemCollection<AmtPipelineStateRecord> mPipelineStates;
		public AmtEncoderItemCollection<AmtPipelineStateRecord> PipelineStates
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

		private readonly AmtEncoderItemCollection<AmtDepthBiasRecord> mDepthBias;
		public AmtEncoderItemCollection<AmtDepthBiasRecord> DepthBias
		{
			get
			{
				return mDepthBias;
			}
		}

		private readonly AmtEncoderItemCollection<AmtStencilReferenceRecord> mStencilReferences;
		public AmtEncoderItemCollection<AmtStencilReferenceRecord> StencilReferences
		{
			get
			{
				return mStencilReferences;
			}
		}
	}
}
