using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLCmdBufferRepository : IGLCmdBufferRepository
	{
		private readonly GLCmdBufferStore<IGLGraphicsPipeline> mGraphicsPipelines;
		private readonly GLCmdBufferStore<GLCmdViewportParameter> mViewports;
		private readonly GLCmdBufferStore<GLCmdDescriptorSetParameter> mDescriptorSets;
		private readonly GLCmdBufferStore<GLCmdScissorParameter> mScissors;
		private readonly GLCmdBufferStore<GLCmdIndexBufferParameter> mIndexBuffers;
		private readonly GLCmdBufferStore<GLCmdVertexBufferParameter> mVertexBuffers;
		private readonly GLCmdBufferStore<float> mLineWidths;
		private readonly GLCmdBufferStore<GLCmdDepthBiasParameter> mDepthBias;
		private readonly GLCmdBufferStore<GLCmdDepthBoundsParameter> mDepthBounds;
		private readonly GLCmdBufferStore<int> mBackCompareMasks;
		private readonly GLCmdBufferStore<int> mFrontCompareMasks;
		private readonly GLCmdBufferStore<int> mFrontReferences;
		private readonly GLCmdBufferStore<int> mBackWriteMasks;
		private readonly GLCmdBufferStore<int> mFrontWriteMasks;
		private readonly GLCmdBufferStore<MgColor4f> mBlendConstants;
		private readonly GLCmdBufferStore<int> mBackReferences;
		private readonly GLCmdBufferStore<GLGraphicsPipelineBlendColorState> mColorBlends;

		public GLCmdBufferRepository ()
		{
			mClearables = new List<IGLCmdBufferStoreResettable> ();

			mGraphicsPipelines = new GLCmdBufferStore<IGLGraphicsPipeline>();
			mClearables.Add (mGraphicsPipelines);

			mViewports = new GLCmdBufferStore<GLCmdViewportParameter>();
			mClearables.Add (mViewports);

			mDescriptorSets = new GLCmdBufferStore<GLCmdDescriptorSetParameter>();
			mClearables.Add (mDescriptorSets);

			mScissors = new GLCmdBufferStore<GLCmdScissorParameter>();
			mClearables.Add (mScissors);

			mIndexBuffers = new GLCmdBufferStore<GLCmdIndexBufferParameter>();
			mClearables.Add (mIndexBuffers);

			mVertexBuffers = new GLCmdBufferStore<GLCmdVertexBufferParameter>();
			mClearables.Add (mVertexBuffers);

			mLineWidths = new GLCmdBufferStore<float> ();
			mClearables.Add (mLineWidths);

			mDepthBias = new GLCmdBufferStore<GLCmdDepthBiasParameter> ();
			mClearables.Add (mDepthBias);

			mDepthBounds = new GLCmdBufferStore<GLCmdDepthBoundsParameter> ();
			mClearables.Add (mDepthBounds);

			mFrontCompareMasks = new GLCmdBufferStore<int> ();
			mClearables.Add (mFrontCompareMasks);

			mBackCompareMasks = new GLCmdBufferStore<int> ();
			mClearables.Add (mBackCompareMasks);

			mFrontReferences = new GLCmdBufferStore<int> (); 
			mClearables.Add (mFrontReferences);

			mBackReferences = new GLCmdBufferStore<int> (); 
			mClearables.Add (mBackReferences);

			mBackWriteMasks = new GLCmdBufferStore<int> (); 
			mClearables.Add (mBackWriteMasks);

			mFrontWriteMasks = new GLCmdBufferStore<int> (); 
			mClearables.Add (mFrontWriteMasks);

			mBlendConstants = new GLCmdBufferStore<MgColor4f> (); 
			mClearables.Add (mBlendConstants);

			mColorBlends = new GLCmdBufferStore<GLGraphicsPipelineBlendColorState> (); 
			mClearables.Add (mColorBlends);
		}
		private List<IGLCmdBufferStoreResettable> mClearables;

		public IGLCmdBufferStore<MgColor4f> BlendConstants { 
			get {
				return mBlendConstants;
			}
		}

		public IGLCmdBufferStore<int> BackWriteMasks { 
			get
			{
				return mBackWriteMasks;
			}
		}

		public IGLCmdBufferStore<int> FrontWriteMasks { 
			get {
				return mFrontWriteMasks;
			}
		}

		public IGLCmdBufferStore<int> BackReferences { 
			get {
				return mBackReferences;
			}
		}

		public IGLCmdBufferStore<int> FrontReferences {	
			get {
				return mFrontReferences;
			}
		}

		public IGLCmdBufferStore<int> FrontCompareMasks { 
			get {
				return mFrontCompareMasks;
			}
		}

		public IGLCmdBufferStore<int> BackCompareMasks { 
			get {
				return mBackCompareMasks;
			}
		}

		public IGLCmdBufferStore<GLCmdVertexBufferParameter> VertexBuffers { 
			get {
				return mVertexBuffers;
			}
		}
		
		public IGLCmdBufferStore<GLCmdIndexBufferParameter> IndexBuffers {
			get
			{
				return mIndexBuffers;
			}
		}

		public IGLCmdBufferStore<IGLGraphicsPipeline> GraphicsPipelines {
			get {
				return mGraphicsPipelines;
			}
		}

		public IGLCmdBufferStore<GLCmdViewportParameter> Viewports {
			get {
				return mViewports;
			}
		}

		public IGLCmdBufferStore<GLCmdDescriptorSetParameter> DescriptorSets { 
			get
			{
				return mDescriptorSets;		
			}
		}

		public IGLCmdBufferStore<GLCmdScissorParameter> Scissors {
			get
			{
				return mScissors;
			}
		}

		public IGLCmdBufferStore<float> LineWidths { 
			get {
				return mLineWidths;
			}
		}
		public IGLCmdBufferStore<GLCmdDepthBiasParameter> DepthBias { 
			get {
				return mDepthBias;
			}
		}
		public IGLCmdBufferStore<GLCmdDepthBoundsParameter> DepthBounds {
			get
			{
				return mDepthBounds;
			}
		}

		public IGLCmdBufferStore<GLGraphicsPipelineBlendColorState> ColorBlends {
			get
			{
				return mColorBlends;
			}
		}

		public bool MapRepositoryFields(ref GLCmdDrawCommand command)
		{
			IGLGraphicsPipeline pipeline = null;
			if (GraphicsPipelines.LastValue (ref pipeline))
			{
				command.DescriptorSet = DescriptorSets.LastIndex ();
				command.IndexBuffer = IndexBuffers.LastIndex ();
				command.VertexBuffer = VertexBuffers.LastIndex ();
				command.Scissors = Scissors.LastIndex ();
				command.Viewports = Viewports.LastIndex ();

				// add defaults
				command.Pipeline = GraphicsPipelines.LastIndex ();

				command.BackCompareMask = BackCompareMasks.LastIndex ();
				command.FrontCompareMask = FrontCompareMasks.LastIndex ();

				command.BackReference = BackReferences.LastIndex ();
				command.FrontReference = FrontReferences.LastIndex ();

				command.BackWriteMask = BackWriteMasks.LastIndex ();
				command.FrontWriteMask = FrontWriteMasks.LastIndex ();

				command.DepthBias = DepthBias.LastIndex ();
				command.DepthBounds = DepthBounds.LastIndex ();

				command.BlendConstants = BlendConstants.LastIndex ();

				command.LineWidth = LineWidths.LastIndex ();
				return true;
			}
			else
			{
				return false;
			}
		}

		public void Clear ()
		{
			foreach (var item in mClearables)
			{
				item.Clear ();
			}
		}

		public void PushGraphicsPipeline (IGLGraphicsPipeline glPipeline)
		{
			GraphicsPipelines.Add (glPipeline);

//			BlendConstants.Add(glPipeline.BlendConstants);
//
//			BackWriteMasks.Add(glPipeline.Back.WriteMask);
//			FrontWriteMasks.Add(glPipeline.Front.WriteMask);
//			BackReferences.Add(glPipeline.Back.Reference);
//			FrontReferences.Add(glPipeline.Front.Reference);
//			FrontCompareMasks.Add(glPipeline.Front.CompareMask);
//			BackCompareMasks.Add(glPipeline.Back.CompareMask);
//
//			Viewports.Add (glPipeline.Viewports);
//			Scissors.Add(glPipeline.Scissors);
//			LineWidths.Add(glPipeline.LineWidth);
//			DepthBias.Add(
//				new GLCmdDepthBiasParameter{
//					DepthBiasConstantFactor = glPipeline.DepthBiasConstantFactor,
//					DepthBiasClamp = glPipeline.DepthBiasClamp,
//					DepthBiasSlopeFactor = glPipeline.DepthBiasSlopeFactor});
//			DepthBounds.Add(
//				new GLCmdDepthBoundsParameter
//				{
//					MinDepthBounds = glPipeline.MinDepthBounds,
//					MaxDepthBounds = glPipeline.MaxDepthBounds,
//				}
//			);
//
//			DescriptorSets.Add (
//				new GLCmdDescriptorSetParameter {
//					Bindpoint = MgPipelineBindPoint.GRAPHICS,
//					DescriptorSets = new MgDescriptorSet[]{},
//					DynamicOffsets = new uint [] {},
//					FirstSet = 0,
//				}
//			);
//			ColorBlends.Add (glPipeline.ColorBlends);
		}

		public void PushScissors (uint firstScissor, MgRect2D[] pScissors)
		{
			IGLGraphicsPipeline pipeline = null;
			if (GraphicsPipelines.LastValue (ref pipeline))
			{
				GLCmdScissorParameter basis = null;
				if ( 
					Scissors.LastValue(ref basis)
					&& (pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.SCISSOR) == GLGraphicsPipelineDynamicStateFlagBits.SCISSOR
				)
				{
					var delta = new GLCmdScissorParameter (firstScissor, pScissors);
					// NO CHANGES ALLOWED WITHOUT DYNAMIC STATES
					var combined = basis.Merge (delta);
					Scissors.Add (combined);
				}
			}

		}

		public void PushViewports (uint firstViewport, MgViewport[] pViewports)
		{
			IGLGraphicsPipeline pipeline = null;
			if (GraphicsPipelines.LastValue (ref pipeline))
			{
				GLCmdViewportParameter basis = null;
				if ( 
					Viewports.LastValue(ref basis)
					&& (pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.VIEWPORT) == GLGraphicsPipelineDynamicStateFlagBits.VIEWPORT
					)
				{
					var delta = new GLCmdViewportParameter(firstViewport, pViewports);
					// NO CHANGES ALLOWED WITHOUT DYNAMIC STATES
					var combined = basis.Merge (delta);
					Viewports.Add (combined);
				}
			}
		}

		public void PushDepthBias (float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			IGLGraphicsPipeline pipeline = null;
			if (GraphicsPipelines.LastValue (ref pipeline))
			{
				if (
					(pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS)
						== GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS)
				{
					DepthBias.Add (new GLCmdDepthBiasParameter {
						DepthBiasConstantFactor = depthBiasConstantFactor,
						DepthBiasClamp = depthBiasClamp,
						DepthBiasSlopeFactor = depthBiasSlopeFactor,
					});
				}
			}
		}

		#region Stencil values

		public void SetStencilReference(MgStencilFaceFlagBits face, uint mask)
		{
			IGLGraphicsPipeline pipeline = null;
			if 
			(
				GraphicsPipelines.LastValue (ref pipeline) 
				&& 
				(
					(pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE)
						== GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE
				)						
			)
			{			
				if ((face & MgStencilFaceFlagBits.FRONT_BIT) > 0)
				{
					FrontReferences.Add ((int)mask);
				}

				if ((face & MgStencilFaceFlagBits.BACK_BIT) > 0)
				{
					BackReferences.Add ((int)mask);
				}
			}
		}

		public void SetWriteMask(MgStencilFaceFlagBits face, uint mask)
		{
			IGLGraphicsPipeline pipeline = null;
			if
			(
				GraphicsPipelines.LastValue (ref pipeline)
				&&
				(
					(pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK)
						== GLGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK
				)
			)
			{	

				if ((face & MgStencilFaceFlagBits.FRONT_BIT) > 0)
				{
					FrontWriteMasks.Add ((int)mask);
				}

				if ((face & MgStencilFaceFlagBits.BACK_BIT) > 0)
				{
					BackWriteMasks.Add ((int)mask);
				}
			}
		}

		public void SetCompareMask(MgStencilFaceFlagBits face, uint mask)
		{
			IGLGraphicsPipeline pipeline = null;
			if
			(
				GraphicsPipelines.LastValue (ref pipeline)
				&&
				(
					(pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK)
						== GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK
				)
			)
			{	
				if ((face & MgStencilFaceFlagBits.FRONT_BIT) > 0)
				{
					FrontCompareMasks.Add ((int)mask);
				}

				if ((face & MgStencilFaceFlagBits.BACK_BIT) > 0)
				{
					BackCompareMasks.Add ((int)mask);
				}
			}
		}

		public void PushLineWidth (float lineWidth)
		{
			IGLGraphicsPipeline pipeline = null;
			if (
				GraphicsPipelines.LastValue (ref pipeline)
				&&
				(
					(pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH)
					== GLGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH
				))
			{
				LineWidths.Add (lineWidth);
			}
		}

		public void PushDepthBounds (float minDepthBounds, float maxDepthBounds)
		{
			IGLGraphicsPipeline pipeline = null;
			if (
				GraphicsPipelines.LastValue (ref pipeline)
				&&
				(
					(pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS)
					== GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS
				))
			{
				DepthBounds.Add (
					new GLCmdDepthBoundsParameter{
						MinDepthBounds = minDepthBounds,
						MaxDepthBounds = maxDepthBounds}
				);
			}
		}

		public void PushBlendConstants (MgColor4f blendConstants)
		{
			IGLGraphicsPipeline pipeline = null;
			if (
				GraphicsPipelines.LastValue (ref pipeline)
				&&
				(
					(pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS)
					== GLGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS
				))
			{
				BlendConstants.Add (blendConstants);
			}
		}

		#endregion


	}
}

