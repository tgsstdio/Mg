using System;
using Magnesium;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
    public class AmtStateRenderer : IAmtStateRenderer
    {
        private readonly IGLCmdBlendEntrypoint mBlend;
        private readonly IGLCmdStencilEntrypoint mStencil;
        private readonly IGLCmdRasterizationEntrypoint mRaster;
        private readonly IGLCmdDepthEntrypoint mDepth;
        private readonly IGLCmdShaderProgramCache mCache;
        private readonly IGLCmdScissorsEntrypoint mScissor;
        private readonly IGLCmdDrawEntrypoint mRender;
        private readonly IGLErrorHandler mErrHandler;
        private readonly IGLCmdClearEntrypoint mClear;

        public AmtStateRenderer(
            IGLCmdBlendEntrypoint blend,
            IGLCmdStencilEntrypoint stencil,
            IGLCmdRasterizationEntrypoint raster,
            IGLCmdDepthEntrypoint depth,
            IGLCmdShaderProgramCache cache,
            IGLCmdScissorsEntrypoint scissor,
            IGLCmdDrawEntrypoint render,
            IGLCmdClearEntrypoint clear,
            IGLErrorHandler errHandler
        )
        {
            mCache = cache;
            mBlend = blend;
            mStencil = stencil;
            mRaster = raster;
            mDepth = depth;
            mScissor = scissor;
            mRender = render;
            mClear = clear;
            mErrHandler = errHandler;
        }

        #region BeginRenderPass methods

        GLQueueRendererClearValueState mPastClearValues;
        public void BeginRenderpass(AmtBeginRenderpassRecord pass)
        {
            // Clear color plus attachment binding
            ApplyClearBuffers(pass.ClearState, pass.Bitmask);

            // Framebuffer stuff here
        }

        void ApplyClearBuffers(GLCmdClearValuesParameter clearState, GLQueueClearBufferMask combinedMask)
        {
            if (clearState.Attachments.Length > 0)
            {
                // TODO : use clear buffers 
                foreach (var state in clearState.Attachments)
                {
                    if (state.Attachment.LoadOp == MgAttachmentLoadOp.CLEAR)
                    {
                        if (state.Attachment.AttachmentType == GLClearAttachmentType.COLOR_INT)
                        {
                            var clearValue = state.Color;
                            if (!mPastClearValues.ClearColor.Equals(state))
                            {
                                mClear.SetClearColor(clearValue);
                                mPastClearValues.ClearColor = clearValue;
                            }
                        }
                        else if (state.Attachment.AttachmentType == GLClearAttachmentType.COLOR_FLOAT)
                        {
                            var clearValue = state.Color;
                            if (!mPastClearValues.ClearColor.Equals(clearValue))
                            {
                                mClear.SetClearColor(clearValue);
                                mPastClearValues.ClearColor = clearValue;
                            }
                        }
                        else if (state.Attachment.AttachmentType == GLClearAttachmentType.COLOR_UINT)
                        {
                            var clearValue = state.Color;
                            //GL.ClearColor (clearValue.X, clearValue.Y, clearValue.Z, clearValue.W);
                            if (!mPastClearValues.ClearColor.Equals(clearValue))
                            {
                                mClear.SetClearColor(clearValue);
                                mPastClearValues.ClearColor = clearValue;
                            }
                        }
                        else if (state.Attachment.AttachmentType == GLClearAttachmentType.DEPTH_STENCIL)
                        {
                            var clearValue = state.Value.DepthStencil;
                            if (Math.Abs(mPastClearValues.DepthValue - clearValue.Depth) > float.Epsilon)
                            {
                                mClear.SetClearDepthValue(clearValue.Depth);
                                mPastClearValues.DepthValue = clearValue.Depth;
                            }
                        }

                    }

                    if (state.Attachment.StencilLoadOp == MgAttachmentLoadOp.CLEAR)
                    {
                        if (state.Attachment.AttachmentType == GLClearAttachmentType.DEPTH_STENCIL)
                        {
                            var clearValue = state.Value.DepthStencil.Stencil;  
                            if (mPastClearValues.StencilValue != clearValue)
                            {
                                mClear.SetClearStencilValue(clearValue);
                                mPastClearValues.StencilValue = clearValue;
                            }
                        }
                    }
                }
                mClear.ClearBuffers(combinedMask);
            }
        }

        #endregion

        #region EndRenderpass methods 

        public void EndRenderpass()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region BindPipeline methods

        public void BindPipeline(AmtBoundPipelineRecordInfo pipelineInfo)
        {
            SetupStatePipelineSettings(pipelineInfo);

            UpdateDynamicStates(pipelineInfo);
        }

        private void SetupStatePipelineSettings(AmtBoundPipelineRecordInfo pipelineInfo)
        {
            // Static pipeline stuff such as depth test enabled
            var pipeline = pipelineInfo.Pipeline;

            if (mCache.ProgramID != pipeline.ProgramID)
            {
                mCache.ProgramID = pipeline.ProgramID;
            }

            SetupBlendSettings(pipeline);
            SetupDepthSettings(pipeline);
            SetupStencilSettings(pipeline);
            SetupRasterizationSettings(pipeline);
        }

        private void UpdateDynamicStates(AmtBoundPipelineRecordInfo pipelineInfo)
        {
            UpdateLineWidth(pipelineInfo.LineWidth);
            UpdateViewports(pipelineInfo.Viewports);
            UpdateScissors(pipelineInfo.Scissors);
            UpdateDepthBias(pipelineInfo.DepthBias);
            UpdateBlendConstants(pipelineInfo.BlendConstants);
            UpdateDepthBounds(pipelineInfo.DepthBounds);
            UpdateFrontStencil(pipelineInfo.FrontStencilInfo);
            UpdateBackStencil(pipelineInfo.BackStencilInfo);

            if (pipelineInfo.FrontStencilWriteMask == pipelineInfo.BackStencilWriteMask)
            {
                UpdateStencilWriteMask(new AmtPipelineStencilWriteInfo
                {
                    Face = MgStencilFaceFlagBits.FRONT_AND_BACK,
                    WriteMask = pipelineInfo.FrontStencilWriteMask,
                });
            }
            else
            {
                UpdateStencilWriteMask(new AmtPipelineStencilWriteInfo
                {
                    Face = MgStencilFaceFlagBits.FRONT_BIT,
                    WriteMask = pipelineInfo.FrontStencilWriteMask,
                });
                UpdateStencilWriteMask(new AmtPipelineStencilWriteInfo
                {
                    Face = MgStencilFaceFlagBits.BACK_BIT,
                    WriteMask = pipelineInfo.BackStencilWriteMask,
                });
            }
        }

        #endregion

        #region UpdateStencilWriteMask methods
        private uint mPastFrontWriteMask;
        private uint mPastBackWriteMask;
        public void UpdateStencilWriteMask(AmtPipelineStencilWriteInfo write)
        {
            if ((write.Face & MgStencilFaceFlagBits.FRONT_AND_BACK) == MgStencilFaceFlagBits.FRONT_AND_BACK)
            {
                if (mPastFrontWriteMask != write.WriteMask || mPastBackWriteMask != write.WriteMask)
                {
                    mStencil.SetStencilWriteMask(MgStencilFaceFlagBits.FRONT_AND_BACK, write.WriteMask);
                    mPastFrontWriteMask = write.WriteMask;
                    mPastBackWriteMask = write.WriteMask;
                }
            }
            else if ((write.Face & MgStencilFaceFlagBits.FRONT_BIT) != 0)
            {
                if (mPastFrontWriteMask != write.WriteMask)
                {
                    mStencil.SetStencilWriteMask(MgStencilFaceFlagBits.FRONT_BIT, write.WriteMask);
                    mPastFrontWriteMask = write.WriteMask;
                }
            }
            else if ((write.Face & MgStencilFaceFlagBits.BACK_BIT) != 0)
            {
                if (mPastBackWriteMask != write.WriteMask)
                {
                    mStencil.SetStencilWriteMask(MgStencilFaceFlagBits.BACK_BIT, write.WriteMask);
                    mPastBackWriteMask = write.WriteMask;
                }
            }
        }

        #endregion

        #region SetupDepthSettings methods

        private GLCmdBufferPipelineItem mPastDepthState;
        private void SetupDepthSettings(IGLGraphicsPipeline pipeline)
        {
            var depthState = ExtractDepthState(pipeline);
            if (ChangesFoundInDepth(depthState))
            {
                ApplyDepthChanges(depthState);
                mPastDepthState = depthState;
            }
        }

        private GLCmdBufferPipelineItem ExtractDepthState(IGLGraphicsPipeline pipeline)
        {
            return new GLCmdBufferPipelineItem
            {
                Flags = pipeline.Flags,                
                DepthState = pipeline.DepthState,
            };
        }

        private bool ChangesFoundInDepth(GLCmdBufferPipelineItem next)
        {
            var mask = GLGraphicsPipelineFlagBits.DepthBufferEnabled
                       | GLGraphicsPipelineFlagBits.DepthBufferWriteEnabled;

            var pastFlags = mask & mPastDepthState.Flags;
            var nextFlags = mask & next.Flags;

            return (pastFlags != nextFlags) || (!mPastDepthState.DepthState.Equals(next.DepthState));
        }

        private void ApplyDepthChanges(GLCmdBufferPipelineItem next)
        {
            var enabled = (next.Flags & GLGraphicsPipelineFlagBits.DepthBufferEnabled) != 0;

            if (mDepth.IsDepthBufferEnabled != enabled)
            {
                if (mDepth.IsDepthBufferEnabled)
                {
                    mDepth.DisableDepthBuffer();
                }
                else
                {
                    mDepth.EnableDepthBuffer();
                }
            }

            var oldDepthWrite = (mPastDepthState.Flags & GLGraphicsPipelineFlagBits.DepthBufferWriteEnabled);
            var newDepthWrite = (next.Flags & GLGraphicsPipelineFlagBits.DepthBufferWriteEnabled);

            var pastDepth = mPastDepthState.DepthState;
            var nextDepth = next.DepthState;

            if ((oldDepthWrite & newDepthWrite) != oldDepthWrite)
            {
                mDepth.SetDepthMask(newDepthWrite != 0);
            }

            if (pastDepth.DepthBufferFunction != nextDepth.DepthBufferFunction)
            {
                mDepth.SetDepthBufferFunc(nextDepth.DepthBufferFunction);
            }
        }

        #endregion

        #region SetupBlendSettings methods

        private GLGraphicsPipelineBlendColorState mPastColorBlendEnums;
        private void SetupBlendSettings(IGLGraphicsPipeline pipeline)
        {
            if (ChangesFoundInBlend(pipeline.ColorBlendEnums))
            {
                ApplyBlendChanges(pipeline.ColorBlendEnums);
                mPastColorBlendEnums = pipeline.ColorBlendEnums;
            }
        }

        private bool ChangesFoundInBlend(GLGraphicsPipelineBlendColorState next)
        {
            if (mPastColorBlendEnums == null && next != null)
            {
                return true;
            }

            if (mPastColorBlendEnums != null && next == null)
            {
                return false;
            }

            if (mPastColorBlendEnums.Attachments.Length != next.Attachments.Length)
                return true;

            if (mPastColorBlendEnums.LogicOpEnable != next.LogicOpEnable)
                return true;

            if (mPastColorBlendEnums.LogicOp != next.LogicOp)
                return true;

            for (uint i = 0; i < next.Attachments.Length; ++i)
            {
                if (!mPastColorBlendEnums.Attachments[i].Equals(next.Attachments[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private void ApplyBlendChanges(GLGraphicsPipelineBlendColorState current)
        {
            if (mPastColorBlendEnums.LogicOpEnable != current.LogicOpEnable || mPastColorBlendEnums.LogicOp != current.LogicOp)
            {
                mBlend.EnableLogicOp(current.LogicOpEnable);
                mBlend.LogicOp(current.LogicOp);
            }

            uint leftSize = (uint)mPastColorBlendEnums.Attachments.Length;
            uint rightSize = (uint)current.Attachments.Length;

            uint fullLoop = Math.Max(leftSize, rightSize);

            for (uint i = 0; i < fullLoop; ++i)
            {
                bool hasPastValue = (i < leftSize);
                bool hasNextValue = (i < rightSize);

                if (hasPastValue && hasNextValue)
                {
                    var past = mPastColorBlendEnums.Attachments[i];
                    var next = current.Attachments[i];

                    //					bool blendEnabled = !(next.Value.SrcColorBlendFactor == MgBlendFactor.ONE &&
                    //					                    next.Value.DstColorBlendFactor == MgBlendFactor.ZERO &&
                    //					                    next.Value.SrcAlphaBlendFactor == MgBlendFactor.ONE &&
                    //					                    next.Value.DstAlphaBlendFactor == MgBlendFactor.ZERO);

                    if (past.BlendEnable != next.BlendEnable)
                    {
                        mBlend.EnableBlending(i, next.BlendEnable);
                    }

                    if (next.SrcColorBlendFactor != past.SrcColorBlendFactor ||
                        next.DstColorBlendFactor != past.DstColorBlendFactor ||
                        next.SrcAlphaBlendFactor != past.SrcAlphaBlendFactor ||
                        next.DstAlphaBlendFactor != past.DstAlphaBlendFactor)
                    {
                        mBlend.ApplyBlendSeparateFunction(
                            i,
                            next.SrcColorBlendFactor,
                            next.DstColorBlendFactor,
                            next.SrcAlphaBlendFactor,
                            next.DstAlphaBlendFactor);
                    }

                    if (past.ColorWriteMask != next.ColorWriteMask)
                    {
                        mBlend.SetColorMask(i, next.ColorWriteMask);
                    }
                }
                else if (!hasPastValue && hasNextValue)
                {
                    var next = current.Attachments[i];
                    //					bool blendEnabled = !(next.Value.SrcColorBlendFactor == MgBlendFactor.ONE &&
                    //						next.Value.DstColorBlendFactor == MgBlendFactor.ZERO &&
                    //						next.Value.SrcAlphaBlendFactor == MgBlendFactor.ONE &&
                    //						next.Value.DstAlphaBlendFactor == MgBlendFactor.ZERO);

                    mBlend.EnableBlending(i, next.BlendEnable);

                    mBlend.ApplyBlendSeparateFunction(
                        i,
                        next.SrcColorBlendFactor,
                        next.DstColorBlendFactor,
                        next.SrcAlphaBlendFactor,
                        next.DstAlphaBlendFactor);

                    mBlend.SetColorMask(i, next.ColorWriteMask);
                }
            }
        }

        #endregion

        #region SetupStencilSettings methods

        private void SetupStencilSettings(IGLGraphicsPipeline pipeline)
        {
            var currentStencil = ExtractStencilValues(pipeline);
            if (ChangesFoundInStencil(mPastStencilInfo, currentStencil))
            {
                ApplyStencilChanges(mPastStencilInfo, currentStencil);
            }
            mPastStencilInfo = currentStencil;
        }

        private void ApplyStencilChanges(
            GLQueueRendererStencilState past,
            GLQueueRendererStencilState next)
        {
            var pastStencil = past.Enums;
            var nextStencil = next.Enums;

            if (past.Front.WriteMask != next.Front.WriteMask)
            {
                mStencil.SetStencilWriteMask(MgStencilFaceFlagBits.FRONT_BIT, next.Front.WriteMask);
            }

            var newStencilEnabled = (next.Flags & GLGraphicsPipelineFlagBits.StencilEnabled);
            if (mStencil.IsStencilBufferEnabled != (newStencilEnabled != 0))
            {
                if (mStencil.IsStencilBufferEnabled)
                {
                    mStencil.DisableStencilBuffer();
                }
                else
                {
                    mStencil.EnableStencilBuffer();

                    if
                    (
                            nextStencil.FrontStencilFail != pastStencil.FrontStencilFail 
                        ||  nextStencil.FrontDepthBufferFail != pastStencil.FrontDepthBufferFail
                        ||  nextStencil.FrontStencilPass != pastStencil.FrontStencilPass
                    )
                    {
                        mStencil.SetFrontFaceStencilOperation(nextStencil.FrontStencilFail, nextStencil.FrontDepthBufferFail, nextStencil.FrontStencilPass);
                    }
                    if
                    (
                            nextStencil.BackStencilFail != pastStencil.BackStencilFail
                        ||  nextStencil.BackDepthBufferFail != pastStencil.BackDepthBufferFail
                        ||  nextStencil.BackStencilPass != pastStencil.BackStencilPass
                    )
                    {
                        mStencil.SetBackFaceStencilOperation(nextStencil.BackStencilFail, nextStencil.BackDepthBufferFail, nextStencil.BackStencilPass);
                    }
                    
                }
            }


        }

        private GLQueueRendererStencilState mPastStencilInfo;
        static GLQueueRendererStencilState ExtractStencilValues(IGLGraphicsPipeline currentPipeline)
        {
            return new GLQueueRendererStencilState
            { 
                Flags = currentPipeline.Flags,
                Enums = currentPipeline.StencilState,
            };
        }

        private static bool ChangesFoundInStencil(
            GLQueueRendererStencilState previous,
            GLQueueRendererStencilState current
        )
        {
            var mask = GLGraphicsPipelineFlagBits.StencilEnabled
                       | GLGraphicsPipelineFlagBits.TwoSidedStencilMode;

            var pastFlags = mask & previous.Flags;
            var nextFlags = mask & current.Flags;

            return (pastFlags != nextFlags) || (!previous.Enums.Equals(current.Enums));
        }

        #endregion

        #region SetupRasterizationSettings methods

        public GLQueueRendererRasterizerState mPastRasterization;
        private void SetupRasterizationSettings(IGLGraphicsPipeline pipeline)
        {
            var currentRasterization = pipeline.Flags;
            if (ChangesFoundInRasterization(currentRasterization))
            {
                ApplyRasterizationChanges(mPastRasterization.Flags, currentRasterization);
            }
            mPastRasterization.Flags = currentRasterization;
        }

        private bool ChangesFoundInRasterization(
            GLGraphicsPipelineFlagBits next)
        {
            var mask = GLGraphicsPipelineFlagBits.CullBackFaces
                | GLGraphicsPipelineFlagBits.CullFrontFaces
                | GLGraphicsPipelineFlagBits.CullingEnabled
                | GLGraphicsPipelineFlagBits.ScissorTestEnabled
                | GLGraphicsPipelineFlagBits.UseCounterClockwiseWindings;

            var pastFlags = mask & mPastRasterization.Flags;
            var nextFlags = mask & next;

            return (pastFlags != nextFlags);
        }

        private void ApplyRasterizationChanges(GLGraphicsPipelineFlagBits previous, GLGraphicsPipelineFlagBits next)
        {
            var mask = GLGraphicsPipelineFlagBits.CullingEnabled;
            if ((previous & mask) != (next & mask))
            {
                if (mRaster.CullingEnabled)
                {
                    mRaster.DisableCulling();
                }
                else
                {
                    mRaster.EnableCulling();
                }
            }

            // culling facing face
            mask = GLGraphicsPipelineFlagBits.CullFrontFaces | GLGraphicsPipelineFlagBits.CullBackFaces;
            if ((previous & mask) != (next & mask))
            {
                mRaster.SetCullingMode(
                    (next & GLGraphicsPipelineFlagBits.CullFrontFaces) > 0
                    , (next & GLGraphicsPipelineFlagBits.CullBackFaces) > 0);
            }

            mask = GLGraphicsPipelineFlagBits.ScissorTestEnabled;
            if ((previous & mask) != (next & mask))
            {
                if (mRaster.ScissorTestEnabled)
                {
                    mRaster.DisableScissorTest();
                }
                else
                {
                    mRaster.EnableScissorTest();
                }
            }

            mask = GLGraphicsPipelineFlagBits.UseCounterClockwiseWindings;
            var nextMaskValue = (next & mask);
            if ((previous & mask) != nextMaskValue)
            {
                mRaster.SetUsingCounterClockwiseWindings(nextMaskValue > 0);
            }
        }

        #endregion

        #region UpdateFrontStencil methods

        AmtGLStencilFunctionInfo mPastFrontStencilInfo;
        public void UpdateFrontStencil(AmtGLStencilFunctionInfo stencilInfo)
        {
            if (
                mPastFrontStencilInfo.StencilFunction != stencilInfo.StencilFunction 
                ||
                mPastFrontStencilInfo.ReferenceMask != stencilInfo.ReferenceMask
                ||
                mPastFrontStencilInfo.CompareMask != stencilInfo.CompareMask
                )
            {
                mStencil.SetFrontFaceCullStencilFunction(stencilInfo.StencilFunction, stencilInfo.ReferenceMask, stencilInfo.CompareMask);
                mPastFrontStencilInfo.ReferenceMask = stencilInfo.ReferenceMask;
                mPastFrontStencilInfo.StencilFunction = stencilInfo.StencilFunction;
                mPastFrontStencilInfo.CompareMask = stencilInfo.CompareMask;
            }
                //if (mPastFrontStencilInfo.WriteMask != stencilInfo.WriteMask)
                //{
                //    mStencil.SetStencilWriteMask(stencilInfo.WriteMask);
                //}
        }

        #endregion

        #region UpdateBackStencil methods

        AmtGLStencilFunctionInfo mPastBackStencilInfo;
        public void UpdateBackStencil(AmtGLStencilFunctionInfo stencilInfo)
        {
            if (
                mPastBackStencilInfo.StencilFunction != stencilInfo.StencilFunction
                ||
                mPastBackStencilInfo.ReferenceMask != stencilInfo.ReferenceMask
                ||
                mPastBackStencilInfo.CompareMask != stencilInfo.CompareMask
                )
            {
                mStencil.SetFrontFaceCullStencilFunction(stencilInfo.StencilFunction, stencilInfo.ReferenceMask, stencilInfo.CompareMask);

                mPastBackStencilInfo.CompareMask = stencilInfo.CompareMask;
                mPastBackStencilInfo.ReferenceMask = stencilInfo.ReferenceMask;
                mPastBackStencilInfo.StencilFunction = stencilInfo.StencilFunction;
            }

                //if (mPastBackStencilInfo.WriteMask != stencilInfo.WriteMask)
                //{
                //    mStencil.SetStencilWriteMask(stencilInfo.WriteMask);
                //}


            
        }

        #endregion

        #region UpdateDepthBounds methods
        private GLCmdDepthBoundsParameter mPastDepthBounds;

        public void UpdateDepthBounds(GLCmdDepthBoundsParameter bounds)
        {
            // GL_EXT_depth_bounds_test
            if (!mPastDepthBounds.Equals(bounds))                
            {
                mDepth.SetDepthBounds(bounds.MinDepthBounds, bounds.MaxDepthBounds);

                mPastDepthBounds = bounds;
            }
        }

        #endregion

        #region UpdateBlendConstants methods

        private MgColor4f mPastBlendConstants;
        public void UpdateBlendConstants(MgColor4f blendConstants)
        {
            if (!mPastBlendConstants.Equals(blendConstants))
            {
                mBlend.SetBlendConstants(blendConstants);
                mPastBlendConstants = blendConstants;
            }
        }

        #endregion

        #region UpdateDepthBias methods

        public void UpdateDepthBias(GLCmdDepthBiasParameter nextDepthBias)
        {
            var previous = mPastRasterization.DepthBias;

            if (Math.Abs(previous.DepthBiasConstantFactor - nextDepthBias.DepthBiasConstantFactor) > float.Epsilon
                || Math.Abs(previous.DepthBiasSlopeFactor - nextDepthBias.DepthBiasSlopeFactor) > float.Epsilon)
            {
                if (nextDepthBias.DepthBiasConstantFactor > 0.0f
                    || nextDepthBias.DepthBiasConstantFactor < 0.0f
                    || nextDepthBias.DepthBiasSlopeFactor < 0.0f
                    || nextDepthBias.DepthBiasSlopeFactor > 0.0f
                    )
                {
                    mRaster.EnablePolygonOffset((float)nextDepthBias.DepthBiasSlopeFactor, (float)nextDepthBias.DepthBiasConstantFactor);
                }
                else
                {
                    mRaster.DisablePolygonOffset();
                }
                mPastRasterization.DepthBias = nextDepthBias;
            }
        }

        #endregion

        #region UpdateScissors methods 

        private GLCmdScissorParameter mPastScissors;
        public void UpdateScissors(GLCmdScissorParameter currentScissors)
        {
            // scissor 
            if (ChangesFoundInScissors(mPastScissors, currentScissors))
            {
                mScissor.ApplyScissors(currentScissors);
                mPastScissors = currentScissors;
            }
        }

        static bool ChangesFoundInScissors(GLCmdScissorParameter pastScissors, GLCmdScissorParameter currentScissors)
        {
            if (pastScissors == null && currentScissors != null)
                return true;

            if (pastScissors != null && currentScissors == null)
                return false;

            return !pastScissors.Equals(currentScissors);
        }

        #endregion

        #region UpdateViewports methods

        private GLCmdViewportParameter mPastViewport;
        public void UpdateViewports(GLCmdViewportParameter currentViewport)
        {
            // viewport
            if (ChangesFoundInViewports(mPastViewport, currentViewport))
            {
                mScissor.ApplyViewports(currentViewport);
                mPastViewport = currentViewport;
            }
        }

        bool ChangesFoundInViewports(GLCmdViewportParameter pastViewport, GLCmdViewportParameter currentViewport)
        {
            if (pastViewport == null && currentViewport != null)
                return true;

            if (pastViewport != null && currentViewport == null)
                return false;

            return !pastViewport.Equals(currentViewport);
        }

        #endregion

        #region UpdateLineWidth methods

        public void UpdateLineWidth(float lineWidth)
        {
            if (Math.Abs(mPastRasterization.LineWidth - lineWidth) > float.Epsilon)
            {
                mRaster.SetLineWidth(lineWidth);
                mPastRasterization.LineWidth = lineWidth;
            }
        }

        #endregion

		public void SetDefault()
		{			
			const int NO_OF_COLOR_ATTACHMENTS = 4;
			mPastColorBlendEnums = mBlend.Initialize (NO_OF_COLOR_ATTACHMENTS);

			var initialStencilValue = mStencil.Initialize ();
			mPastStencilInfo = initialStencilValue;

            mPastFrontWriteMask = initialStencilValue.Front.WriteMask;
            mPastBackWriteMask = initialStencilValue.Back.WriteMask;

            mPastFrontStencilInfo = new AmtGLStencilFunctionInfo
            {
                CompareMask = initialStencilValue.Front.CompareMask,
                ReferenceMask = initialStencilValue.Front.Reference,
                StencilFunction = initialStencilValue.Enums.FrontStencilFunction,
            };

            mPastBackStencilInfo = new AmtGLStencilFunctionInfo
            {
                CompareMask = initialStencilValue.Back.CompareMask,
                ReferenceMask = initialStencilValue.Back.Reference,
                StencilFunction = initialStencilValue.Enums.BackStencilFunction,
            };

            var initialDepthValue = mDepth.Initialize ();
			PreviousPipeline = new GLCmdBufferPipelineItem {
				DepthState = initialDepthValue,
				StencilState = initialStencilValue.Enums,
			};

			mPastRasterization = mRaster.Initialize ();

			mPastClearValues = mClear.Initialize ();
		}

		public GLCmdBufferPipelineItem PreviousPipeline { get ; private set; }
		//public GLCmdBufferDrawItem mPreviousItem;

		public void Render(CmdBufferInstructionSet[] items)
		{
			var pastPipeline = PreviousPipeline;
			var pastStencil = mPastStencilInfo;
			var clearValues = 0;

			var isFirst = true;

			foreach (var instructionSet in items)
			{
				foreach (var drawItem in instructionSet.DrawItems)
				{
					// TODO : bind render target
					var currentPipeline = instructionSet.Pipelines[drawItem.Pipeline];

					CheckProgram (instructionSet, drawItem);

					// Draw here 
					//if ((drawItem.Command & GLCommandBufferFlagBits.CmdDrawIndexedIndirect) == GLCommandBufferFlagBits.CmdDrawIndexedIndirect)
     //               {
     //                   DrawIndexedIndirect(drawItem);
     //               }
     //               else if ((drawItem.Command & GLCommandBufferFlagBits.CmdDrawIndexed) == GLCommandBufferFlagBits.CmdDrawIndexed)
     //               {
     //                   DrawIndexed(drawItem);
     //               }
     //               else if ((drawItem.Command & GLCommandBufferFlagBits.CmdDrawIndirect) == GLCommandBufferFlagBits.CmdDrawIndirect)
     //               {
     //                   DrawIndirect(drawItem);
     //               }
                    //else
                    //{
                    //    Draw(drawItem);
                    //}

                    //					pastState = instructionSet;
                    pastPipeline = currentPipeline;
				}
			}
			PreviousPipeline = pastPipeline;
		}

        public void Draw(GLCmdInternalDraw drawItem)
        {
            mRender.DrawArrays(drawItem.Topology, drawItem.FirstVertex, drawItem.VertexCount, drawItem.InstanceCount, drawItem.FirstInstance);
        }

        public void DrawIndirect(GLCmdInternalDrawIndirect drawItem)
        {
            mRender.DrawArraysIndirect(drawItem.Topology, drawItem.Indirect, drawItem.DrawCount, drawItem.Stride);
        }

        public void DrawIndexed(GLCmdInternalDrawIndexed drawItem)
        {
            mRender.DrawIndexed(drawItem.Topology, drawItem.IndexType, drawItem.FirstIndex, drawItem.IndexCount, drawItem.InstanceCount, drawItem.VertexOffset);
        }

        public void DrawIndexedIndirect(GLCmdInternalDrawIndexedIndirect drawItem)
        {
            mRender.DrawIndexedIndirect(drawItem.Topology, drawItem.IndexType, drawItem.Indirect, drawItem.DrawCount, drawItem.Stride);
        }

        public void CheckProgram(CmdBufferInstructionSet instructionSet, GLCmdBufferDrawItem drawItem)
		{
			// bind program
			if (mCache.ProgramID != drawItem.ProgramID)
			{
				mCache.ProgramID = drawItem.ProgramID;
				mCache.VBO = drawItem.VBO;
				mCache.DescriptorSet = instructionSet.DescriptorSets [drawItem.DescriptorSet];
				mCache.BindDescriptorSet ();
			}
			else
			{
				mCache.VBO = drawItem.VBO;

				if (mCache.DescriptorSetIndex != drawItem.DescriptorSet)
				{
					// TODO : FIX ME
					mCache.DescriptorSet = instructionSet.DescriptorSets [drawItem.DescriptorSet];

					if (mCache.DescriptorSet != null)
					{
						mCache.BindDescriptorSet ();
					}
				}
			}

			// bind constant buffers
//			if (currentProgram.GetBufferMask () != nextState.BufferMask)
//			{
//				currentProgram.BindMask (mBuffers);
//			}
			// bind uniforms



		}

        public void BindVertexArrays(object vao)
        {
            throw new NotImplementedException();
        }
    }
}

