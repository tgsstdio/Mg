using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdGraphicsEncoder : IGLCmdGraphicsEncoder
    {
        private readonly IGLCmdEncoderContextSorter mInstructions;
        private readonly IGLCmdVBOEntrypoint mVBO;
        private IGLDescriptorSetBinder mDSBinder;

        // NOT SURE IF THIS IS NEEDED IN HERE
        //public CmdBufferInstructionSet InstructionSet { get; private set; }
        private GLCmdGraphicsBag mBag;

        public GLCmdGraphicsEncoder
        (
            IGLCmdEncoderContextSorter instructions,
            GLCmdGraphicsBag bag,
            IGLCmdVBOEntrypoint vbo,
            IGLDescriptorSetBinder dsBinder
        )
        {
            mInstructions = instructions;
            mBag = bag;
            mVBO = vbo;
            mDSBinder = dsBinder;
        }

        public GLCmdGraphicsGrid AsGrid()
        {
            return new GLCmdGraphicsGrid
            {
                Renderpasses = mBag.Renderpasses.ToArray(),
                Pipelines = mBag.Pipelines.ToArray(),
                StencilWrites = mBag.StencilWrites.ToArray(),
                Viewports = mBag.Viewports.ToArray(),
                BlendConstants = mBag.BlendConstants.ToArray(),
                DepthBias = mBag.DepthBias.ToArray(),
                DepthBounds = mBag.DepthBounds.ToArray(),
                LineWidths = mBag.LineWidths.ToArray(),
                Scissors = mBag.Scissors.ToArray(),             
                DrawIndexedIndirects = mBag.DrawIndexedIndirects.ToArray(),
                DrawIndexeds = mBag.DrawIndexeds.ToArray(),
                DrawIndirects = mBag.DrawIndirects.ToArray(),
                Draws = mBag.Draws.ToArray(),
                StencilFunctions = mBag.StencilFunctions.ToArray(),
                VAOs = mBag.VAOs.ToArray(),
                DescriptorSets = mBag.DescriptorSets.ToArray(),                
            };
        }

        #region BeginRenderPass methods

        private GLCmdBeginRenderpassRecord mBoundRenderPass;
        public void BeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
        {
            mBoundRenderPass =  InitializeRenderpassInfo(pRenderPassBegin);

            var nextIndex = mBag.Renderpasses.Push(mBoundRenderPass);

            var instruction = new GLCmdEncodingInstruction
            {
                Category = GLCmdEncoderCategory.Graphics,
                Index = nextIndex,
                Operation = CmdBeginRenderPass,
            };

            mInstructions.Add(instruction);
        }

        private static void CmdBeginRenderPass(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.Renderpasses;
            Debug.Assert(items != null);
            var passInfo = items[arg2];
            Debug.Assert(passInfo != null);
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.BeginRenderpass(passInfo);
        }

        GLCmdBeginRenderpassRecord InitializeRenderpassInfo(MgRenderPassBeginInfo pass)
        {
            Debug.Assert(pass != null);

            var glPass = (IGLRenderPass)pass.RenderPass;

            var noOfAttachments = glPass.AttachmentFormats == null ? 0 : glPass.AttachmentFormats.Length;
            var noOfClearValues = pass.ClearValues == null ? 0 : pass.ClearValues.Length;

            var finalLength = Math.Min(noOfAttachments, noOfClearValues);

            var finalValues = new List<GLCmdClearValueArrayItem>();

            GLQueueClearBufferMask combinedMask = 0;
            for (var i = 0; i < finalLength; ++i)
            {
                var attachment = glPass.AttachmentFormats[i];

                MgColor4f colorValue = new MgColor4f();

                switch (attachment.AttachmentType)
                {
                    case GLClearAttachmentType.COLOR_INT:
                        colorValue = ExtractColorI(attachment, pass.ClearValues[i].Color.Int32);
                        combinedMask |= GLQueueClearBufferMask.Color;
                        break;
                    case GLClearAttachmentType.COLOR_UINT:
                        colorValue = ExtractColorUi(attachment, pass.ClearValues[i].Color.Uint32);
                        combinedMask |= GLQueueClearBufferMask.Color;
                        break;
                    case GLClearAttachmentType.COLOR_FLOAT:
                        colorValue = pass.ClearValues[i].Color.Float32;
                        combinedMask |= GLQueueClearBufferMask.Color;
                        break;
                    case GLClearAttachmentType.DEPTH_STENCIL:
                        //clearValue.Value = pass.ClearValues[i];
                        combinedMask |= GLQueueClearBufferMask.Depth;
                        break;
                    default:
                        break;
                }

                var clearValue = new GLCmdClearValueArrayItem
                {
                    Attachment = attachment,
                    Color = colorValue,
                    Value = pass.ClearValues[i]
                };

                finalValues.Add(clearValue);

            }

            return new GLCmdBeginRenderpassRecord
            {
                Bitmask = combinedMask,
                ClearState = new GLCmdClearValuesParameter
                {
                    Attachments = finalValues.ToArray(),
                }
            };
        }

        public static MgColor4f ExtractColorUi(GLClearAttachmentInfo attachment, MgVec4Ui initialValue)
        {
            return new MgColor4f
            {
                R = Math.Min((float)initialValue.X, attachment.Divisor) / attachment.Divisor,
                G = Math.Min((float)initialValue.Y, attachment.Divisor) / attachment.Divisor,
                B = Math.Min((float)initialValue.Z, attachment.Divisor) / attachment.Divisor,
                A = Math.Min((float)initialValue.W, attachment.Divisor) / attachment.Divisor,
            };
        }

        public static MgColor4f ExtractColorI(GLClearAttachmentInfo attachment, MgVec4i initialValue)
        {
            return new MgColor4f
            {
                R = Math.Max(Math.Min((float)initialValue.X, attachment.Divisor), -attachment.Divisor) / attachment.Divisor,
                G = Math.Max(Math.Min((float)initialValue.Y, attachment.Divisor), -attachment.Divisor) / attachment.Divisor,
                B = Math.Max(Math.Min((float)initialValue.Z, attachment.Divisor), -attachment.Divisor) / attachment.Divisor,
                A = Math.Max(Math.Min((float)initialValue.W, attachment.Divisor), -attachment.Divisor) / attachment.Divisor,
            };
        }


        #endregion

        #region BindDescriptorSets methods

        public void BindDescriptorSets(IMgPipelineLayout layout, uint firstSet, uint descriptorSetCount, IMgDescriptorSet[] pDescriptorSets, uint[] pDynamicOffsets)
        {
            mDSBinder.Bind(MgPipelineBindPoint.GRAPHICS, layout, firstSet, descriptorSetCount, pDescriptorSets, pDynamicOffsets);

            if (mDSBinder.IsInvalid)
            {
                InvalidateDescriptorSets();
            }
        }

        private void InvalidateDescriptorSets()
        {
            mPastDescriptorSet = null;
        }

        private GLCmdDescriptorSetParameter mPastDescriptorSet;
        private void PushBackDescriptorSetIfRequired()
        {
            if (mCurrentPipeline == null)
                return;

            Debug.Assert(mDSBinder != null);

            if (mPastDescriptorSet == null)
            {
                mPastDescriptorSet = new GLCmdDescriptorSetParameter
                {
                    Bindpoint = MgPipelineBindPoint.GRAPHICS,    
                    Layout = mDSBinder.BoundPipelineLayout,  
                    DescriptorSet = mDSBinder.BoundDescriptorSet,              
                    DynamicOffsets = mDSBinder.BoundDynamicOffsets,
                };

                var nextIndex = mBag.DescriptorSets.Push(mPastDescriptorSet);

                mInstructions.Add(new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdBindDescriptorSets,
                });
            }
        }

        private void CmdBindDescriptorSets(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.DescriptorSets;
            Debug.Assert(items != null);
            var ds = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            // NO NEED FOR removed assert checks for ds.DescriptorSet, ds.DynamicOffsets, ds.Layout as nulls/empty sets are allowed
            renderer.BindDescriptorSets(ds);
        }

        #endregion

        #region BindPipeline methods

        private IGLGraphicsPipeline mCurrentPipeline;
        public void BindPipeline(IMgPipeline pipeline)
        {
            Debug.Assert(pipeline != null);

            var glPipeline = (IGLGraphicsPipeline)pipeline;

            mCurrentPipeline = glPipeline;

            var pipelineInfo = InitializePipelineInfo(); 
            var nextIndex = mBag.Pipelines.Push(pipelineInfo);

            var instruction = new GLCmdEncodingInstruction
            {
                Category = GLCmdEncoderCategory.Graphics,
                Index = nextIndex,
                Operation = CmdBindPipeline,
            };

            if (mBoundRenderPass != null)
            {
                mInstructions.Add(instruction);
            }
        }

        private GLCmdBoundPipelineRecordInfo InitializePipelineInfo()
        {
            // ONLY if pipeline ATTACHED and dynamic state has been set
            var frontReference = mCurrentPipeline.Front.Reference;
            var backReference = mCurrentPipeline.Back.Reference;

            if (
                (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE)
                    == GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE
               )
            {
                frontReference = mFrontReference;
                backReference = mBackReference;
            }

            var backCompare = mCurrentPipeline.Back.CompareMask;
            var frontCompare = mCurrentPipeline.Front.CompareMask;

            if (
                (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK)
                    == GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK
               )
            {
                backCompare = mBackCompare;
                frontCompare = mFrontCompare;
            }

            var backWriteMask = mCurrentPipeline.Back.WriteMask;
            var frontWriteMask = mCurrentPipeline.Front.WriteMask;

            if (
                (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK)
                    == GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK
               )
            {
                backWriteMask = mBackWrite;
                frontWriteMask = mFrontWrite;
            }

            return new GLCmdBoundPipelineRecordInfo
            {
                Pipeline = mCurrentPipeline,
                LineWidth = FetchLineWidth(),
                BlendConstants = FetchBlendConstants(),
                BackStencilInfo = new GLCmdStencilFunctionInfo
                {
                    ReferenceMask = backReference,
                    CompareMask = backCompare,
                    StencilFunction = mCurrentPipeline.StencilState.BackStencilFunction,
                },
                FrontStencilInfo = new GLCmdStencilFunctionInfo
                {
                    ReferenceMask = frontReference,
                    CompareMask = frontCompare,
                    StencilFunction = mCurrentPipeline.StencilState.FrontStencilFunction,
                },
                DepthBias = FetchDepthBias(),
                DepthBounds = FetchDepthBounds(),
                Scissors = FetchScissors(),
                Viewports = FetchViewports(),
                BackStencilWriteMask = backWriteMask,
                FrontStencilWriteMask = frontWriteMask,                
            };
        }

        private GLCmdScissorParameter FetchScissors()
        {
            var scissors = mCurrentPipeline.Scissors;
            if (
                (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.SCISSOR)
                    == GLGraphicsPipelineDynamicStateFlagBits.SCISSOR
            )
            {
                scissors = mPastScissors;
            }

            return scissors;
        }

        private GLCmdViewportParameter FetchViewports()
        {
            var viewports = mCurrentPipeline.Viewports;
            if (
                (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.VIEWPORT)
                    == GLGraphicsPipelineDynamicStateFlagBits.VIEWPORT
            )
            {
                viewports = mPastViewports;
            }

            return viewports;
        }

        private GLCmdDepthBoundsParameter FetchDepthBounds()
        {
            var depthBounds = new GLCmdDepthBoundsParameter
            {
                MinDepthBounds = mCurrentPipeline.MinDepthBounds,
                MaxDepthBounds = mCurrentPipeline.MaxDepthBounds,
            };

            if (
                    (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS)
                        == GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS
            )
            {
                depthBounds.MinDepthBounds = mMinDepthBounds;
                depthBounds.MaxDepthBounds = mMaxDepthBounds;
            }

            return depthBounds;
        }

        private GLCmdDepthBiasParameter FetchDepthBias()
        {
            var depthBias = new GLCmdDepthBiasParameter
            {
                DepthBiasClamp = mCurrentPipeline.DepthBiasClamp,
                DepthBiasConstantFactor = mCurrentPipeline.DepthBiasConstantFactor,
                DepthBiasSlopeFactor = mCurrentPipeline.DepthBiasSlopeFactor,
            };

            if (
                    (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS)
                        == GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS
                   )
            {
                depthBias.DepthBiasClamp = mDepthBiasClamp;
                depthBias.DepthBiasConstantFactor = mDepthBiasConstantFactor;
                depthBias.DepthBiasSlopeFactor = mDepthBiasSlopeFactor;
            }

            return depthBias;
        }

        private MgColor4f FetchBlendConstants()
        {
            var blendConstants = mCurrentPipeline.BlendConstants;
            if (
                    (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS)
                        == GLGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS
                   )
            {
                blendConstants = mBlendConstants;
            }

            return blendConstants;
        }

        private float FetchLineWidth()
        {
            var lineWidth = mCurrentPipeline.LineWidth;
            if (
                (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH)
                    == GLGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH
               )
            {
                lineWidth = mLineWidth;
            }
            return lineWidth;
        }

        private void CmdBindPipeline(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.Pipelines;
            Debug.Assert(items != null);
            var pipelineInfo = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.BindPipeline(pipelineInfo);
        }

        #endregion

        public void Clear()
        {
            mFrontCompare = ~0U;
            mBackCompare = ~0U;
            mFrontWrite = ~0U;
            mBackWrite = ~0U;
            mFrontReference = 0;
            mBackReference = 0;
            mDepthBiasClamp = 0f;
            mDepthBiasSlopeFactor = 0f;
            mDepthBiasConstantFactor = 0f;

            mBoundRenderPass = null;

            mPastScissors = null;

            mPastViewports = null;

            InvalidateBackStencil();
            InvalidateFrontStencil();

            InvalidateDescriptorSets();
            mDSBinder.Clear();
        }

        bool StoreDrawCommand()
        {
            if (mCurrentPipeline == null)
            {
                return false;
            }            
            
            if (mBoundRenderPass == null)
            {
                throw new InvalidOperationException("Command must be made inside a Renderpass. ");
            }

            // if vbo is missing, generate new one
            PushVertexArrayIfRequired();

            // if front stencil is missing, generate new one
            PushFrontStencilIfRequired();

            // if back stencil is missing, generate new one
            PushBackStencilIfRequired();

            // if descriptor sets is missing, generate new one
            PushBackDescriptorSetIfRequired();

            return true;            
        }

        private GLCmdStencilFunctionInfo mPastBackStencil;
        private void PushBackStencilIfRequired()
        {
            if (mCurrentPipeline == null)
                return;

            if (mPastBackStencil == null)
            {
                mPastBackStencil = new GLCmdStencilFunctionInfo
                {
                    CompareMask = mBackCompare,
                    ReferenceMask = mBackReference,
                    StencilFunction = mCurrentPipeline.StencilState.BackStencilFunction,
                };

                var nextIndex = mBag.StencilFunctions.Push(mPastBackStencil);

                mInstructions.Add(new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdUpdateBackStencil,
                });
            }
        }

        private void CmdUpdateBackStencil(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.StencilFunctions;
            Debug.Assert(items != null);
            var func = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.UpdateBackStencil(func);
        }

        private void InvalidateBackStencil()
        {
            mPastBackStencil = null;
        }

        private GLCmdStencilFunctionInfo mPastFrontStencil;
        private void PushFrontStencilIfRequired()
        {
            if (mCurrentPipeline == null)
                return;

            if (mPastFrontStencil == null)
            {               
                mPastFrontStencil = new GLCmdStencilFunctionInfo
                {
                    CompareMask = mFrontCompare,
                    ReferenceMask = mFrontReference,
                    StencilFunction = mCurrentPipeline.StencilState.FrontStencilFunction,
                };

                var nextIndex = mBag.StencilFunctions.Push(mPastFrontStencil);

                mInstructions.Add(new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdUpdateFrontStencil,
                });
            }
        }

        private void CmdUpdateFrontStencil(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.StencilFunctions;
            Debug.Assert(items != null);
            var func = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.UpdateFrontStencil(func);
        }

        private void InvalidateFrontStencil()
        {
            mPastFrontStencil = null;
        }

        // NEED TO MERGE 
        private GLCmdVertexBufferObject mCurrentVertexArray;
        private void PushVertexArrayIfRequired()
        {
            // create new vbo
            if (mCurrentVertexArray == null)
            {
                mCurrentVertexArray = GenerateVBO();

                var nextIndex = mBag.VAOs.Push(mCurrentVertexArray);

                mInstructions.Add(new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdBindVertexBuffers,
                });
            }
            //else
            //{
            //    bool useSameBuffers = (command.IndexBuffer.HasValue)
            //        ? mCurrentVertexArray.Matches(
            //            command.VertexBuffer.Value,
            //            command.IndexBuffer.Value)
            //        : mCurrentVertexArray.Matches(
            //            command.VertexBuffer.Value);

            //    if (!useSameBuffers)
            //    {
            //        mCurrentVertexArray = GenerateVBO(userSettings, command, pipeline);
            //        VBOs.Add(currentVertexArray);
            //    }
            //}            
        }

        private void CmdBindVertexBuffers(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.VAOs;
            Debug.Assert(items != null);
            var vao = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.BindVertexArrays(vao);
        }

        private GLCmdVertexBufferParameter mBoundVertexBuffer;
        public void BindVertexBuffers(uint firstBinding, IMgBuffer[] pBuffers, ulong[] pOffsets)
        {
            if (pBuffers == null)
                throw new ArgumentNullException(nameof(pBuffers));

            mBoundVertexBuffer = new GLCmdVertexBufferParameter
            {
                firstBinding = firstBinding,
                pBuffers = pBuffers,
                pOffsets = pOffsets,
            };  
        }

        private GLCmdIndexBufferParameter mBoundIndexBuffer;
        public void BindIndexBuffer(IMgBuffer buffer, ulong offset, MgIndexType indexType)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));           

            mBoundIndexBuffer = new GLCmdIndexBufferParameter
            {
                buffer = (IGLBuffer)buffer,
                offset = offset,
                indexType = indexType,
            };
        }

        GLCmdVertexBufferObject GenerateVBO()
        {
            var vertexData = mBoundVertexBuffer;
            var noOfBindings = mCurrentPipeline.VertexInput.Bindings.Length;
            var bufferIds = new uint[noOfBindings];
            var offsets = new long[noOfBindings];

            for (uint i = 0; i < vertexData.pBuffers.Length; ++i)
            {
                uint index = i + vertexData.firstBinding;
                var buffer = vertexData.pBuffers[index] as IGLBuffer;
                // SILENT error
                if ((buffer.Usage & MgBufferUsageFlagBits.VERTEX_BUFFER_BIT) == MgBufferUsageFlagBits.VERTEX_BUFFER_BIT)
                {
                    bufferIds[i] = buffer.BufferId;
                    offsets[i] = (vertexData.pOffsets != null) ? (long)vertexData.pOffsets[i] : 0L;
                }
                else
                {
                    bufferIds[i] = 0;
                    offsets[i] = 0;
                }
            }

            var vbo = mVBO.GenerateVBO();
            foreach (var attribute in mCurrentPipeline.VertexInput.Attributes)
            {
                var bufferId = bufferIds[attribute.Binding];
                var binding = mCurrentPipeline.VertexInput.Bindings[attribute.Binding];
                mVBO.AssociateBufferToLocation(vbo, attribute.Location, bufferId, offsets[attribute.Binding], binding.Stride);
                // GL.VertexArrayVertexBuffer (vertexArray, location, bufferId, new IntPtr (offset), (int)binding.Stride);

                if (attribute.Function == GLVertexAttribFunction.FLOAT)
                {
                    // direct state access
                    mVBO.BindFloatVertexAttribute(vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.IsNormalized, attribute.Offset);

                    //GL.VertexArrayAttribFormat (vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.IsNormalized, attribute.Offset);
                }
                else if (attribute.Function == GLVertexAttribFunction.INT)
                {
                    mVBO.BindIntVertexAttribute(vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.Offset);

                    //GL.VertexArrayAttribIFormat (vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.Offset);
                }
                else if (attribute.Function == GLVertexAttribFunction.DOUBLE)
                {
                    mVBO.BindDoubleVertexAttribute(vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.Offset);
                    //GL.VertexArrayAttribLFormat (vbo, attribute.Location, attribute.Size, (All)attribute.PointerType, attribute.Offset);
                }
                mVBO.SetupVertexAttributeDivisor(vbo, attribute.Location, attribute.Divisor);
                //GL.VertexArrayBindingDivisor (vbo, attribute.Location, attribute.Divisor);
            }

            if (mBoundIndexBuffer != null)
            {
                var indexBuffer = mBoundIndexBuffer.buffer;
                if (indexBuffer != null && ((indexBuffer.Usage & MgBufferUsageFlagBits.INDEX_BUFFER_BIT) == MgBufferUsageFlagBits.INDEX_BUFFER_BIT))
                {
                    mVBO.BindIndexBuffer(vbo, indexBuffer.BufferId);
                    //GL.VertexArrayElementBuffer (vbo, indexBuffer.BufferId);
                }
            }

            return new GLCmdVertexBufferObject(vbo, mVBO);
        }

        #region Draw methods 

        public void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
        {
            //void glDrawArraysInstancedBaseInstance(GLenum mode​, GLint first​, GLsizei count​, GLsizei primcount​, GLuint baseinstance​);
            //mDrawCommands.Add (mIncompleteDrawCommand);
            // first => firstVertex
            // count => vertexCount
            // primcount => instanceCount Specifies the number of instances of the indexed geometry that should be drawn.
            // baseinstance => firstInstance Specifies the base instance for use in fetching instanced vertex attributes.


            if (StoreDrawCommand())
            {
                Debug.Assert(mCurrentPipeline != null);
                var draw = new GLCmdInternalDraw
                {                    
                    Topology = mCurrentPipeline.Topology,
                    VertexCount = vertexCount,
                    InstanceCount = instanceCount,
                    FirstVertex = firstVertex,
                    FirstInstance = firstInstance,
                };

                var nextIndex = mBag.Draws.Push(draw);

                mInstructions.Add(new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdDraw,
                });
            }
        }

        private static void CmdDraw(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.Draws;
            Debug.Assert(items != null);
            var draw = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.Draw(draw);
        }

        #endregion

        #region DrawIndexed methods

        public void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
        {
            // void glDrawElementsInstancedBaseVertex(GLenum mode​, GLsizei count​, GLenum type​, GLvoid *indices​, GLsizei primcount​, GLint basevertex​);
            // count => indexCount Specifies the number of elements to be rendered. (divide by elements)
            // indices => firstIndex Specifies a byte offset (cast to a pointer type) (multiple by data size)
            // primcount => instanceCount Specifies the number of instances of the indexed geometry that should be drawn.
            // basevertex => vertexOffset Specifies a constant that should be added to each element of indices​ when chosing elements from the enabled vertex arrays.
            // TODO : need to handle negetive offset
            //mDrawCommands.Add (mIncompleteDrawCommand);

            if (StoreDrawCommand())
            {
                Debug.Assert(mCurrentPipeline != null);
                Debug.Assert(mBoundIndexBuffer != null);

                var draw = new GLCmdInternalDrawIndexed
                {
                    Topology = mCurrentPipeline.Topology,
                    IndexType = mBoundIndexBuffer.indexType,
                    IndexCount = indexCount,
                    InstanceCount = instanceCount,
                    FirstIndex = firstIndex,
                    VertexOffset = vertexOffset,
                    FirstInstance = firstInstance,
                };

                var nextIndex = mBag.DrawIndexeds.Push(draw);

                mInstructions.Add(new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdDrawIndexed,
                });
            }
        }

        private static void CmdDrawIndexed(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.DrawIndexeds;
            Debug.Assert(items != null);
            var draw = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.DrawIndexed(draw);
        }

        #endregion

        #region DrawIndexedIndirect methods

        public void DrawIndexedIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
        {
            Debug.Assert(buffer != null);

            //			typedef struct VkDrawIndexedIndirectCommand {
            //				uint32_t    indexCount;
            //				uint32_t    instanceCount;
            //				uint32_t    firstIndex;
            //				int32_t     vertexOffset;
            //				uint32_t    firstInstance;
            //			} VkDrawIndexedIndirectCommand;
            // void glMultiDrawElementsIndirect(GLenum mode​, GLenum type​, const void *indirect​, GLsizei drawcount​, GLsizei stride​);
            // indirect  => buffer + offset (IntPtr)
            // drawcount => drawcount
            // stride => stride
            //			glDrawElementsInstancedBaseVertexBaseInstance(mode,
            //				cmd->count,
            //				type,
            //				cmd->firstIndex * size-of-type,
            //				cmd->instanceCount,
            //				cmd->baseVertex,
            //				cmd->baseInstance);
            //			typedef  struct {
            //				uint  count;
            //				uint  instanceCount;
            //				uint  firstIndex;
            //				uint  baseVertex; // TODO: negetive index
            //				uint  baseInstance;
            //			} DrawElementsIndirectCommand;
            //mDrawCommands.Add (mIncompleteDrawCommand);

            if (offset >= (ulong)int.MaxValue)
            {
                throw new InvalidOperationException();
            }

            if (StoreDrawCommand())
            {
                Debug.Assert(mCurrentPipeline != null);
                Debug.Assert(mBoundIndexBuffer != null);

                var glBuffer = ((IGLBuffer)buffer);

                Debug.Assert(glBuffer != null);

                var indirect = IntPtr.Add(glBuffer.Source, (int)offset);

                var draw = new GLCmdInternalDrawIndexedIndirect
                {
                    Indirect = indirect,
                    Topology = mCurrentPipeline.Topology,
                    IndexType = mBoundIndexBuffer.indexType,
                    DrawCount = drawCount,
                    Stride = stride,
                };

                var nextIndex = mBag.DrawIndexedIndirects.Push(draw);

                mInstructions.Add(new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdDrawIndexedIndirect,
                });
            }
        }

        private void CmdDrawIndexedIndirect(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.DrawIndexedIndirects;
            Debug.Assert(items != null);
            var draw = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.DrawIndexedIndirect(draw);
        }

        #endregion

        #region DrawIndirect methods

        public void DrawIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
        {
            Debug.Assert(buffer != null);

            // ARB_multi_draw_indirect
            //			typedef struct VkDrawIndirectCommand {
            //				uint32_t    vertexCount;
            //				uint32_t    instanceCount; 
            //				uint32_t    firstVertex; 
            //				uint32_t    firstInstance;
            //			} VkDrawIndirectCommand;
            // glMultiDrawArraysIndirect 
            //void glMultiDrawArraysIndirect(GLenum mode​, const void *indirect​, GLsizei drawcount​, GLsizei stride​);
            // indirect => buffer + offset IntPtr
            // drawCount => drawCount
            // stride => stride
            //			typedef  struct {
            //				uint  count;
            //				uint  instanceCount;
            //				uint  first;
            //				uint  baseInstance;
            //			} DrawArraysIndirectCommand;
            //mDrawCommands.Add (mIncompleteDrawCommand);  

            if (offset >= (ulong)int.MaxValue)
            {
                throw new InvalidOperationException();
            }

            if (StoreDrawCommand())
            {
                Debug.Assert(mCurrentPipeline != null);

                var glBuffer = ((IGLBuffer)buffer);

                Debug.Assert(glBuffer != null);

                var indirect = IntPtr.Add(glBuffer.Source, (int) offset);

                var draw = new GLCmdInternalDrawIndirect
                {
                    Topology = mCurrentPipeline.Topology,
                    Indirect = indirect,
                    DrawCount = drawCount,
                    Stride = stride,
                 };

                var nextIndex = mBag.DrawIndirects.Push(draw);

                mInstructions.Add(new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdDrawIndirect,
                });
            }
        }

        private static void CmdDrawIndirect(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.DrawIndirects;
            Debug.Assert(items != null);
            var draw = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.DrawIndirect(draw);
        }

        #endregion

        public void EndRenderPass()
        {
            mBoundRenderPass = null;
        }

        public void NextSubpass(MgSubpassContents contents)
        {
      
        }

        #region SetBlendConstants methods

        private MgColor4f mBlendConstants;
        public void SetBlendConstants(MgColor4f blendConstants)
        {
            mBlendConstants = blendConstants;
            // ONLY if 
            // no pipeline has been set
            // OR pipeline ATTACHED and dynamic state has been set
            if
            (
                (mCurrentPipeline == null)
                ||
                (mCurrentPipeline != null
                    &&
                    (
                        (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS)
                            == GLGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS
                    )
                )
            )
            {
                var nextIndex = mBag.BlendConstants.Push(mBlendConstants);

                var instruction = new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetBlendConstants,
                };

                mInstructions.Add(instruction);
            }
        }

        private void CmdSetBlendConstants(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.BlendConstants;
            Debug.Assert(items != null);
            var blends = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.UpdateBlendConstants(blends);
        }

        #endregion

        #region SetDepthBias methods

        float mDepthBiasConstantFactor;
        float mDepthBiasClamp;
        float mDepthBiasSlopeFactor;
        public void SetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
        {
            mDepthBiasConstantFactor = depthBiasConstantFactor;
            mDepthBiasClamp = depthBiasClamp;
            mDepthBiasSlopeFactor = depthBiasSlopeFactor;

            // ONLY if 
            // no pipeline has been set
            // OR pipeline ATTACHED and dynamic state has been set
            if
            (
                (mCurrentPipeline == null)
                ||
                (mCurrentPipeline != null
                    &&
                    (
                        (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS)
                            == GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS
                    )
                )
            )
            {
                var bias = new GLCmdDepthBiasParameter
                {
                    DepthBiasClamp = mDepthBiasClamp,
                    DepthBiasConstantFactor = mDepthBiasConstantFactor,
                    DepthBiasSlopeFactor = mDepthBiasSlopeFactor
                };

                var nextIndex = mBag.DepthBias.Push(bias);

                var instruction = new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetDepthBias,
                };

                mInstructions.Add(instruction);
            }
        }

        private void CmdSetDepthBias(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var bias = grid.DepthBias[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.UpdateDepthBias(bias);
        }

        #endregion

        #region SetDepthBounds methods

        private float mMinDepthBounds;
        private float mMaxDepthBounds;

        public void SetDepthBounds(float minDepthBounds, float maxDepthBounds)
        {
            mMinDepthBounds = minDepthBounds;
            mMaxDepthBounds = maxDepthBounds;

            // ONLY if 
            // no pipeline has been set
            // OR pipeline ATTACHED and dynamic state has been set
            if
            (
                (mCurrentPipeline == null)
                ||
                (mCurrentPipeline != null
                    &&
                    (
                        (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS)
                            == GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS
                    )
                )
            )
            {
                var bounds = new GLCmdDepthBoundsParameter
                {
                    MinDepthBounds = mMinDepthBounds,
                    MaxDepthBounds = mMaxDepthBounds
                };

                var nextIndex = mBag.DepthBounds.Push(bounds);

                var instruction = new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetDepthBounds
                };

                mInstructions.Add(instruction);
            }
        }

        private static void CmdSetDepthBounds(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var bounds = grid.DepthBounds[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.UpdateDepthBounds(bounds);
        }

        #endregion

        #region SetLineWidth methods

        private float mLineWidth;
        public void SetLineWidth(float lineWidth)
        {
            mLineWidth = lineWidth;

            // ONLY if 
            // no pipeline has been set
            // OR pipeline ATTACHED and dynamic state has been set
            if
            (
                (mCurrentPipeline == null)
                ||
                (mCurrentPipeline != null
                    &&
                    (
                        (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH)
                            == GLGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH
                    )
                )
            )
            {
                var nextIndex = mBag.LineWidths.Push(mLineWidth);

                var instruction = new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetLineWidth
                };

                mInstructions.Add(instruction);
            }
        }

        private static void CmdSetLineWidth(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var lineWidth = grid.LineWidths[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.UpdateLineWidth(lineWidth);
        }

        #endregion

        #region SetScissor methods

        public void SetScissor(uint firstScissor, MgRect2D[] pScissors)
        {
            mPastScissors = new GLCmdScissorParameter(firstScissor, pScissors);

            // ONLY if 
            // no pipeline has been set
            // OR pipeline ATTACHED and dynamic state has been set
            if
            (
                (mCurrentPipeline == null)
                ||
                (mCurrentPipeline != null
                    &&
                    (
                        (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.SCISSOR)
                            == GLGraphicsPipelineDynamicStateFlagBits.SCISSOR
                    )
                )
            )
            {
                var nextIndex = mBag.Scissors.Push(mPastScissors);

                var instruction = new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetScissor
                };

                mInstructions.Add(instruction);
            }
        }

        private void CmdSetScissor(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var scissors = grid.Scissors[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.UpdateScissors(scissors);
        }

        #endregion

        #region SetStencilCompareMask methods

        private uint mBackCompare;
        private uint mFrontCompare;

        public void SetStencilCompareMask(MgStencilFaceFlagBits faceMask, uint compareMask)
        {
            bool frontChange = ((faceMask & MgStencilFaceFlagBits.FRONT_BIT) == MgStencilFaceFlagBits.FRONT_BIT);
            bool backChange = ((faceMask & MgStencilFaceFlagBits.BACK_BIT) == MgStencilFaceFlagBits.BACK_BIT);

            if (backChange)
            {
                if (mBackCompare != compareMask)
                {
                    InvalidateBackStencil();

                    mBackCompare = compareMask;                    
                }
            }
            if (frontChange)
            {
                if (mFrontCompare != compareMask)
                {
                    InvalidateFrontStencil();

                    mFrontCompare = compareMask;                    
                }
            }
        }



        #endregion

        #region SetStencilReference methods

        private int mBackReference;
        private int mFrontReference;

        public void SetStencilReference(MgStencilFaceFlagBits faceMask, uint reference)
        {
            bool frontChange = ((faceMask & MgStencilFaceFlagBits.FRONT_BIT) == MgStencilFaceFlagBits.FRONT_BIT);
            bool backChange = ((faceMask & MgStencilFaceFlagBits.BACK_BIT) == MgStencilFaceFlagBits.BACK_BIT);

            if (backChange)
            {
                if (mBackReference != reference)
                {
                    InvalidateBackStencil();

                    unchecked
                    {
                        mBackReference = (int)reference;
                    }
                }
            }
            if (frontChange)
            {
                if (mFrontReference != reference)
                {
                    InvalidateFrontStencil();

                    unchecked
                    {
                        mFrontReference = (int)reference;
                    }
                }
            }
        }

        #endregion

        #region SetStencilWriteMask methods 

        private uint mFrontWrite;
        private uint mBackWrite;

        public void SetStencilWriteMask(MgStencilFaceFlagBits faceMask, uint writeMask)
        {
            bool frontChange = ((faceMask & MgStencilFaceFlagBits.FRONT_BIT) == MgStencilFaceFlagBits.FRONT_BIT);
            bool backChange = ((faceMask & MgStencilFaceFlagBits.BACK_BIT) == MgStencilFaceFlagBits.BACK_BIT);

            if (backChange)
            {
                mBackWrite = writeMask;                
            }
            if (frontChange)
            {
               mFrontWrite = writeMask;                
            }

            // ONLY if 
            // no pipeline has been set
            // OR pipeline ATTACHED and dynamic state has been set
            if
            (
                (mCurrentPipeline == null)
                ||
                (mCurrentPipeline != null
                    &&
                    (
                        (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK)
                            == GLGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK
                    )
                )
            )
            {
                if (frontChange && backChange)
                {
                    var stencilWrite = new GLCmdPipelineStencilWriteInfo
                    {
                        Face = MgStencilFaceFlagBits.FRONT_AND_BACK,
                        WriteMask = writeMask,
                    };

                    var nextIndex = mBag.StencilWrites.Push(stencilWrite);

                    var instruction = new GLCmdEncodingInstruction
                    {
                        Category = GLCmdEncoderCategory.Graphics,
                        Index = nextIndex,
                        Operation = CmdSetStencilWrite
                    };

                    mInstructions.Add(instruction);
                }
                else if (backChange)
                {
                    var stencilWrite = new GLCmdPipelineStencilWriteInfo
                    {
                        Face = MgStencilFaceFlagBits.BACK_BIT,
                        WriteMask = writeMask,
                    };

                    var nextIndex = mBag.StencilWrites.Push(stencilWrite);

                    var instruction = new GLCmdEncodingInstruction
                    {
                        Category = GLCmdEncoderCategory.Graphics,
                        Index = nextIndex,
                        Operation = CmdSetStencilWrite
                    };

                    mInstructions.Add(instruction);
                }

                if (frontChange)
                {
                    var stencilWrite = new GLCmdPipelineStencilWriteInfo
                    {
                        Face = MgStencilFaceFlagBits.FRONT_BIT,
                        WriteMask = writeMask,
                    };

                    var nextIndex = mBag.StencilWrites.Push(stencilWrite);

                    var instruction = new GLCmdEncodingInstruction
                    {
                        Category = GLCmdEncoderCategory.Graphics,
                        Index = nextIndex,
                        Operation = CmdSetStencilWrite
                    };

                    mInstructions.Add(instruction);
                }
            }
        }

        private void CmdSetStencilWrite(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.StencilWrites;
            Debug.Assert(items != null);
            var writeInfo = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.UpdateStencilWriteMask(writeInfo);
        }

        #endregion

        #region SetViewports methods

        private GLCmdViewportParameter mPastViewports;
        private GLCmdScissorParameter mPastScissors;

        public void SetViewports(uint firstViewport, MgViewport[] pViewports)
        {
            mPastViewports = new GLCmdViewportParameter(firstViewport, pViewports);

            // ONLY if 
            // no pipeline has been set
            // OR pipeline ATTACHED and dynamic state has been set
            if
            (
                (mCurrentPipeline == null)
                ||
                (mCurrentPipeline != null
                    &&
                    (
                        (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.VIEWPORT)
                            == GLGraphicsPipelineDynamicStateFlagBits.VIEWPORT
                    )
                )
            )
            {
                var nextIndex = mBag.Viewports.Push(mPastViewports);

                var instruction = new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetViewports
                };

                mInstructions.Add(instruction);
            }
        }

        private void CmdSetViewports(GLCmdCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var items = grid.Viewports;
            Debug.Assert(items != null);
            var viewports = items[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.UpdateViewports(viewports);
        }

        #endregion
    }
}
