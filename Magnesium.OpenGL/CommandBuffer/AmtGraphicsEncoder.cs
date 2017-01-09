using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.OpenGL
{
    public class AmtGraphicsEncoder : IAmtGraphicsEncoder
    {
        private readonly IGLCmdBufferRepository mRepository;
        private readonly IAmtEncoderContextSorter mInstructions;
        private readonly List<GLCmdRenderPassCommand> mRenderPasses = new List<GLCmdRenderPassCommand>();

        // NOT SURE IF THIS IS NEEDED IN HERE
        private List<GLCmdDrawCommand> mIncompleteDraws = new List<GLCmdDrawCommand>();
        public CmdBufferInstructionSet InstructionSet { get; private set; }
        private AmtGraphicsBag mBag;

        public AmtGraphicsEncoder
        (
            IAmtEncoderContextSorter instructions, 
            IGLCmdBufferRepository repository,
            AmtGraphicsBag bag
        )
        {
            mInstructions = instructions;
            mRepository = repository;
            mBag = bag;
        }

        public AmtGraphicsGrid AsGrid()
        {
            return new AmtGraphicsGrid
            {
                Renderpasses = mBag.Renderpasses.ToArray(),
            };
        }

        #region BeginRenderPass methods

        private GLCmdRenderPassCommand mIncompleteRenderPass;
        public void BeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
        {
            mIncompleteRenderPass = new GLCmdRenderPassCommand();
            mIncompleteRenderPass.Origin = pRenderPassBegin.RenderPass;
            mIncompleteRenderPass.ClearValues = pRenderPassBegin.ClearValues;
            mIncompleteRenderPass.Contents = contents;

            var beginInfo =  InitialiseRenderpassInfo(pRenderPassBegin);

            var nextIndex = mBag.Renderpasses.Push(beginInfo);

            var instruction = new AmtEncodingInstruction
            {
                Category = AmtEncoderCategory.Graphics,
                Index = nextIndex,
                Operation = CmdBeginRenderPass,
            };

            mInstructions.Add(instruction);
        }

        private static void CmdBeginRenderPass(AmtCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var passInfo = grid.Renderpasses[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.BeginRenderpass(passInfo);
        }

        AmtBeginRenderpassRecord InitialiseRenderpassInfo(MgRenderPassBeginInfo pass)
        {
            Debug.Assert(pass != null);

            var glPass = (IGLRenderPass)pass.RenderPass;

            var noOfAttachments = glPass.AttachmentFormats == null ? 0 : glPass.AttachmentFormats.Length;
            var noOfClearValues = pass.ClearValues == null ? 0 : pass.ClearValues.Length;

            var finalLength = Math.Min(noOfAttachments, noOfClearValues);

            var finalValues = new List<GLClearValueArrayItem>();

            GLQueueClearBufferMask combinedMask = 0;
            for (var i = 0; i < finalLength; ++i)
            {
                var attachment = glPass.AttachmentFormats[i];

                var clearValue = new GLClearValueArrayItem
                {
                    Attachment = attachment,
                    Value = pass.ClearValues[i]
                };

                finalValues.Add(clearValue);

                switch (attachment.AttachmentType)
                {
                    case GLClearAttachmentType.COLOR_INT:
                        clearValue.Color = ExtractColorI(attachment, pass.ClearValues[i].Color.Int32);
                        combinedMask |= GLQueueClearBufferMask.Color;
                        break;
                    case GLClearAttachmentType.COLOR_UINT:
                        clearValue.Color = ExtractColorUi(attachment, pass.ClearValues[i].Color.Uint32);
                        combinedMask |= GLQueueClearBufferMask.Color;
                        break;
                    case GLClearAttachmentType.COLOR_FLOAT:
                        clearValue.Color = pass.ClearValues[i].Color.Float32;
                        combinedMask |= GLQueueClearBufferMask.Color;
                        break;
                    case GLClearAttachmentType.DEPTH_STENCIL:
                        clearValue.Value = pass.ClearValues[i];
                        combinedMask |= GLQueueClearBufferMask.Depth;
                        break;
                    default:
                        break;
                }

            }

            return new AmtBeginRenderpassRecord
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

        public void BindDescriptorSets(IMgPipelineLayout layout, uint firstSet, uint descriptorSetCount, IMgDescriptorSet[] pDescriptorSets, uint[] pDynamicOffsets)
        {
            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            // TODO: TRANSFORM into more GL specific code
            var parameter = new GLCmdDescriptorSetParameter();
            parameter.Bindpoint = MgPipelineBindPoint.GRAPHICS;
            parameter.Layout = layout;
            parameter.FirstSet = firstSet;
            parameter.DynamicOffsets = pDynamicOffsets;
            parameter.DescriptorSets = pDescriptorSets;

            mRepository.DescriptorSets.Add(parameter);
            var nextIndex = (uint) mRepository.DescriptorSets.LastIndex().Value;

            var instruction = new AmtEncodingInstruction
            {
                Category = AmtEncoderCategory.Graphics,
                Index = nextIndex,
                Operation = CmdBindDescriptorSets,
            };

            mInstructions.Add(instruction);
        }

        private static void CmdBindDescriptorSets(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();   
        }

        #region BindIndexBuffer methods

        public void BindIndexBuffer(IMgBuffer buffer, ulong offset, MgIndexType indexType)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            // TODO: TRANSFORM into more GL specific code
            var param = new GLCmdIndexBufferParameter();
            param.buffer = buffer;
            param.offset = offset;
            param.indexType = indexType;
            mRepository.IndexBuffers.Add(param);

            var nextIndex = (uint)mRepository.IndexBuffers.LastIndex().Value;

            var instruction = new AmtEncodingInstruction
            {
                Category = AmtEncoderCategory.Graphics,
                Index = nextIndex,
                Operation = CmdBindIndexBuffer,
            };

            mInstructions.Add(instruction);
        }

        private static void CmdBindIndexBuffer(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region BindPipeline methods
        private IGLGraphicsPipeline mCurrentPipeline;
        public void BindPipeline(IMgPipeline pipeline)
        {
            Debug.Assert(pipeline != null);

            var glPipeline = (IGLGraphicsPipeline)pipeline;

            mCurrentPipeline = glPipeline;

            var pipelineInfo = InitialisePipelineInfo(); 
            var nextIndex = mBag.Pipelines.Push(pipelineInfo);

            var instruction = new AmtEncodingInstruction
            {
                Category = AmtEncoderCategory.Graphics,
                Index = nextIndex,
                Operation = CmdBindPipeline,
            };

            mInstructions.Add(instruction);
        }

        private AmtBoundPipelineRecordInfo InitialisePipelineInfo()
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

            return new AmtBoundPipelineRecordInfo
            {
                Pipeline = mCurrentPipeline,
                LineWidth = FetchLineWidth(),
                BlendConstants = FetchBlendConstants(),
                BackStencilInfo = new AmtGLStencilFunctionInfo
                {
                    ReferenceMask = backReference,
                    CompareMask = backCompare,
                    WriteMask = backWriteMask,
                    StencilFunction = mCurrentPipeline.StencilState.BackStencilFunction,
                },
                FrontStencilInfo = new AmtGLStencilFunctionInfo
                {
                    ReferenceMask = frontReference,
                    CompareMask = frontCompare,
                    WriteMask = backWriteMask,
                    StencilFunction = mCurrentPipeline.StencilState.FrontStencilFunction,
                },
                DepthBias = FetchDepthBias(),
                DepthBounds = FetchDepthBounds(),
                Scissors = FetchScissors(),
                Viewports = FetchViewports(),
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
                // TODO : init GL scissors parameter
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
                // TODO : init GL viewports parameter
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

        private void CmdBindPipeline(AmtCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var pipelineInfo = grid.Pipelines[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.BindPipeline(pipelineInfo);
        }

        #endregion

        #region BindVertexBuffers methods

        public void BindVertexBuffers(uint firstBinding, IMgBuffer[] pBuffers, ulong[] pOffsets)
        {
            if (pBuffers == null)
                throw new ArgumentNullException(nameof(pBuffers));

            var param = new GLCmdVertexBufferParameter();
            param.firstBinding = firstBinding;
            param.pBuffers = pBuffers;
            param.pOffsets = pOffsets;
            mRepository.VertexBuffers.Add(param);

            var nextIndex = (uint)mRepository.VertexBuffers.LastIndex().Value;

            var instruction = new AmtEncodingInstruction
            {
                Category = AmtEncoderCategory.Graphics,
                Index = nextIndex,
                Operation = CmdBindVertexBuffers,
            };

            mInstructions.Add(instruction);
        }

        private static void CmdBindVertexBuffers(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void Clear()
        {
            mIncompleteRenderPass = null;
            mRepository.Clear();
            mRenderPasses.Clear();
            mIncompleteDraws.Clear();

            if (InstructionSet != null)
            {
                foreach (var vbo in InstructionSet.VBOs)
                {
                    vbo.Dispose();
                }
                InstructionSet = null;
            }
        }

        bool StoreDrawCommand(GLCmdDrawCommand command, out uint nextIndex)
        {
            bool hasPipeline = mRepository.MapRepositoryFields(ref command);

            if (!hasPipeline)
            {
                nextIndex = 0;
                return false;
            }
            
            if (mIncompleteRenderPass == null)
            {
                throw new InvalidOperationException("Command must be made inside a Renderpass. ");
            }

            mIncompleteRenderPass.DrawCommands.Add(command);
            nextIndex = (uint) mIncompleteRenderPass.DrawCommands.Count;
            return true;            
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

            var command = new GLCmdDrawCommand();
            command.Draw = new GLCmdInternalDraw();
            command.Draw.vertexCount = vertexCount;
            command.Draw.instanceCount = instanceCount;
            command.Draw.firstVertex = firstVertex;
            command.Draw.firstInstance = firstInstance;

            uint nextIndex;
            if (StoreDrawCommand(command, out nextIndex))
            {
                mInstructions.Add(new AmtEncodingInstruction
                {
                    Category = AmtEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdDraw,
                });
            }
        }

        private static void CmdDraw(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
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

            var command = new GLCmdDrawCommand();
            command.DrawIndexed = new GLCmdInternalDrawIndexed();
            command.DrawIndexed.indexCount = indexCount;
            command.DrawIndexed.instanceCount = instanceCount;
            command.DrawIndexed.firstIndex = firstIndex;
            command.DrawIndexed.vertexOffset = vertexOffset;
            command.DrawIndexed.firstInstance = firstInstance;

            uint nextIndex;
            if (StoreDrawCommand(command, out nextIndex))
            {
                mInstructions.Add(new AmtEncodingInstruction
                {
                    Category = AmtEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdDrawIndexed,
                });
            }
        }

        private static void CmdDrawIndexed(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
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
            var command = new GLCmdDrawCommand();
            command.DrawIndexedIndirect = new GLCmdInternalDrawIndexedIndirect();
            command.DrawIndexedIndirect.buffer = buffer;
            command.DrawIndexedIndirect.offset = offset;
            command.DrawIndexedIndirect.drawCount = drawCount;
            command.DrawIndexedIndirect.stride = stride;

            uint nextIndex;
            if (StoreDrawCommand(command, out nextIndex))
            {
                mInstructions.Add(new AmtEncodingInstruction
                {
                    Category = AmtEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdDrawIndexedIndirect,
                });
            }
        }

        private void CmdDrawIndexedIndirect(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
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

            var command = new GLCmdDrawCommand();
            command.DrawIndirect = new GLCmdInternalDrawIndirect();
            command.DrawIndirect.buffer = buffer;
            command.DrawIndirect.offset = offset;
            command.DrawIndirect.drawCount = drawCount;
            command.DrawIndirect.stride = stride;

            uint nextIndex;
            if (StoreDrawCommand(command, out nextIndex))
            {
                mInstructions.Add(new AmtEncodingInstruction
                {
                    Category = AmtEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdDrawIndirect,
                });
            }
        }

        private static void CmdDrawIndirect(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void EndRenderPass()
        {
            mRenderPasses.Add(mIncompleteRenderPass);
            mIncompleteRenderPass = null;
        }

        public void NextSubpass(MgSubpassContents contents)
        {
            throw new NotImplementedException();
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
                mRepository.PushBlendConstants(blendConstants);

                var nextIndex = (uint)mRepository.BlendConstants.LastIndex().Value;

                var instruction = new AmtEncodingInstruction
                {
                    Category = AmtEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetBlendConstants,
                };

                mInstructions.Add(instruction);
            }
        }

        private void CmdSetBlendConstants(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
        }

        #endregion

        float mDepthBiasConstantFactor;
        float mDepthBiasClamp;
        float mDepthBiasSlopeFactor;

        #region SetDepthBias methods

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

                mRepository.PushDepthBias(
                    depthBiasConstantFactor,
                    depthBiasClamp,
                    depthBiasSlopeFactor);

                var nextIndex = (uint)mRepository.BlendConstants.LastIndex().Value;

                var instruction = new AmtEncodingInstruction
                {
                    Category = AmtEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetDepthBias,
                };

                mInstructions.Add(instruction);
            }
        }

        private void CmdSetDepthBias(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
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

                mRepository.PushDepthBounds(
                    minDepthBounds,
                    maxDepthBounds
                );

                var nextIndex = (uint)mRepository.DepthBounds.LastIndex().Value;

                var instruction = new AmtEncodingInstruction
                {
                    Category = AmtEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetDepthBounds
                };

                mInstructions.Add(instruction);
            }
        }

        private static void CmdSetDepthBounds(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
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
                mRepository.PushLineWidth(lineWidth);

                var nextIndex = (uint)mRepository.LineWidths.LastIndex().Value;

                var instruction = new AmtEncodingInstruction
                {
                    Category = AmtEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetLineWidth
                };

                mInstructions.Add(instruction);
            }
        }

        private static void CmdSetLineWidth(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region SetScissor methods
        private uint mFirstScissor;
        private MgRect2D[] mScissors;

        public void SetScissor(uint firstScissor, MgRect2D[] pScissors)
        {
            mFirstScissor = firstScissor;
            mScissors = pScissors;

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
                mRepository.PushScissors(firstScissor, pScissors);

                var nextIndex = (uint)mRepository.Scissors.LastIndex().Value;

                var instruction = new AmtEncodingInstruction
                {
                    Category = AmtEncoderCategory.Graphics,
                    Index = nextIndex,
                    Operation = CmdSetScissor
                };

                mInstructions.Add(instruction);
            }
        }

        private void CmdSetScissor(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region SetStencilCompareMask methods

        private int mBackCompare;
        private int mFrontCompare;

        public void SetStencilCompareMask(MgStencilFaceFlagBits faceMask, uint compareMask)
        {
            bool frontChange = ((faceMask & MgStencilFaceFlagBits.FRONT_BIT) == MgStencilFaceFlagBits.FRONT_BIT);
            bool backChange = ((faceMask & MgStencilFaceFlagBits.BACK_BIT) == MgStencilFaceFlagBits.BACK_BIT);

            if (backChange)
            {
                unchecked
                {
                    mBackCompare = (int) compareMask;
                }
            }
            if (frontChange)
            {
                unchecked
                {
                    mFrontCompare = (int) compareMask;
                }
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
                        (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK)
                            == GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK
                    )
                )
            )
            {
                if (backChange)
                {
                    mRepository.SetCompareMask(MgStencilFaceFlagBits.BACK_BIT, compareMask);
                }

                if (frontChange)
                {
                    mRepository.SetCompareMask(MgStencilFaceFlagBits.FRONT_BIT, compareMask);
                }
            }
        }

        private void CmdSetStencilCompareMask(AmtCommandRecording arg1, uint arg2)
        {
            throw new NotImplementedException();
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
                unchecked
                {
                    mBackReference = (int) reference;
                }
            }
            if (frontChange)
            {
                unchecked
                {
                    mFrontReference = (int) reference;
                }
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
                        (mCurrentPipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE)
                            == GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE
                    )
                )
            )
            {
                if (backChange)
                {
                    mRepository.SetStencilReference(MgStencilFaceFlagBits.BACK_BIT, reference);
                }

                if (frontChange)
                {
                    mRepository.SetStencilReference(MgStencilFaceFlagBits.FRONT_BIT, reference);
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
                    var stencilWrite = new AmtPipelineStencilWriteInfo
                    {
                        Face = MgStencilFaceFlagBits.FRONT_AND_BACK,
                        WriteMask = writeMask,
                    };

                    var nextIndex = mBag.StencilWrites.Push(stencilWrite);

                    var instruction = new AmtEncodingInstruction
                    {
                        Category = AmtEncoderCategory.Graphics,
                        Index = nextIndex,
                        Operation = CmdSetStencilWrite
                    };

                    mInstructions.Add(instruction);
                }
                else if (backChange)
                {
                    var stencilWrite = new AmtPipelineStencilWriteInfo
                    {
                        Face = MgStencilFaceFlagBits.BACK_BIT,
                        WriteMask = writeMask,
                    };

                    var nextIndex = mBag.StencilWrites.Push(stencilWrite);

                    var instruction = new AmtEncodingInstruction
                    {
                        Category = AmtEncoderCategory.Graphics,
                        Index = nextIndex,
                        Operation = CmdSetStencilWrite
                    };

                    mInstructions.Add(instruction);
                }

                if (frontChange)
                {
                    var stencilWrite = new AmtPipelineStencilWriteInfo
                    {
                        Face = MgStencilFaceFlagBits.FRONT_BIT,
                        WriteMask = writeMask,
                    };

                    var nextIndex = mBag.StencilWrites.Push(stencilWrite);

                    var instruction = new AmtEncodingInstruction
                    {
                        Category = AmtEncoderCategory.Graphics,
                        Index = nextIndex,
                        Operation = CmdSetStencilWrite
                    };

                    mInstructions.Add(instruction);
                }
            }
        }

        private void CmdSetStencilWrite(AmtCommandRecording arg1, uint arg2)
        {
            var context = arg1.Graphics;
            Debug.Assert(context != null);
            var grid = context.Grid;
            Debug.Assert(grid != null);
            var writeInfo = grid.StencilWrites[arg2];
            var renderer = context.StateRenderer;
            Debug.Assert(renderer != null);
            renderer.SetStencilWriteMask(writeInfo);
        }

        #endregion

        public void SetViewports(uint firstViewport, MgViewport[] pViewports)
        {
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

            }
        }
    }
}
