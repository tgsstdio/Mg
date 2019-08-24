using DryIoc;
using Magnesium;
using Magnesium.OpenGL;
using Magnesium.OpenGL.Internals;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL.GL001.UnitTests
{
    [TestFixture]
    public class UnitTest
    {      
        private IMgQueue queue;
        private IMgCommandBuffer cmdBuf;

        private IMgFramebuffer framebuffer;
        private IMgRenderPass renderpass;
        private IMgPipeline pipeline;

        private IMgBuffer vertexBuffer;
        private IMgBuffer indexBuffer;

        [Test]
        public void Test_GL001()
        {
            uint width = 600;
            uint height = 400;
            MgFormat format = MgFormat.R8G8B8A8_UINT;

            var beginInfo = new MgCommandBufferBeginInfo { Flags = 0, };
            cmdBuf.BeginCommandBuffer(beginInfo);

            var passBeginInfo = new MgRenderPassBeginInfo
            {
                Framebuffer = framebuffer,
                RenderPass = renderpass,
                RenderArea = new MgRect2D
                {
                    Extent = new MgExtent2D
                    {
                        Width = width,
                        Height = height,
                    },
                    Offset = new MgOffset2D
                    {
                        X = 0,
                        Y = 0,
                    }
                },
                ClearValues = new[] 
                {
                    MgClearValue.FromColorAndFormat(format, new MgColor4f(1f, 0, 1f, 1f)),
                    new MgClearValue {
                        DepthStencil = new MgClearDepthStencilValue (1f, 0),
                    }
                },
            };
            cmdBuf.CmdBeginRenderPass(passBeginInfo, MgSubpassContents.INLINE);

            cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, pipeline);

            cmdBuf.CmdBindVertexBuffers(0, new[] { vertexBuffer }, new ulong[] { 0 });
            cmdBuf.CmdBindIndexBuffer(indexBuffer, 0, MgIndexType.UINT32);
            cmdBuf.CmdDrawIndexed(6, 1, 0, 0, 0);

            cmdBuf.CmdEndRenderPass();

            var err = cmdBuf.EndCommandBuffer();

            var submitInfo = new[]
            {
                new MgSubmitInfo
                {
                    CommandBuffers = new []
                    {
                        cmdBuf,
                    }
                }
            };

            queue.QueueSubmit(submitInfo, null);
            queue.QueueWaitIdle();

            // MAKE SURE DOUBLE SUBMISSION WORKS
            queue.QueueSubmit(submitInfo, null);
            queue.QueueWaitIdle();
        }

        private Container mContainer;

        [SetUp]
        public void Preamble()
        {
            // PRIVATE IMPLEMENTATION UNIT TESTING            
            mContainer = new Container();

            mContainer.Register<IMgQueue, GLCmdQueue>(Reuse.Singleton);
            mContainer.Register<IGLCmdStateRenderer, GLCmdStateRenderer>(Reuse.Singleton);

            mContainer.Register<IGLCmdBlendEntrypoint, MockGLCmdBlendEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLCmdStencilEntrypoint, MockGLCmdStencilEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLCmdRasterizationEntrypoint, MockGLCmdRasterizationEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLCmdDepthEntrypoint, MockGLCmdDepthEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLCmdScissorsEntrypoint, MockGLCmdScissorsEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLCmdDrawEntrypoint, MockGLCmdDrawEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLCmdClearEntrypoint, MockGLCmdClearEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLErrorHandler, MockGLErrorHandler>(Reuse.Singleton);


            mContainer.Register<IGLNextCmdShaderProgramCache, GLNextCmdShaderProgramCache>(Reuse.Singleton);
            mContainer.Register<IGLCmdShaderProgramEntrypoint, MockGLCmdShaderProgramEntrypoint>(Reuse.Singleton);


            mContainer.Register<IGLBlitOperationEntrypoint, MockGLBlitOperationEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLSemaphoreEntrypoint, MockGLSemaphoreEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLCmdImageEntrypoint, MockGLCmdImageEntrypoint>(Reuse.Singleton);

            mContainer.Register<IGLCmdBlitEncoder,GLCmdBlitEncoder>(Reuse.Singleton);
            mContainer.Register<GLCmdBlitBag>(Reuse.Singleton);
            mContainer.Register<IGLCmdComputeEncoder,GLCmdComputeEncoder>(Reuse.Singleton);
            mContainer.Register<IGLCmdGraphicsEncoder,GLCmdGraphicsEncoder>(Reuse.Singleton);

            mContainer.Register<GLCmdGraphicsBag>(Reuse.Singleton);
            mContainer.Register<IGLCmdVBOEntrypoint, MockGLCmdVBOEntrypoint>(Reuse.Singleton);
            mContainer.Register<IGLDescriptorSetBinder,GLNextDescriptorSetBinder>(Reuse.Singleton);


            mContainer.Register<IGLCmdEncoderContextSorter,GLCmdIncrementalContextSorter>(Reuse.Singleton);
            mContainer.Register<GLCmdCommandEncoder>(Reuse.Singleton);


            mContainer.Register<GLInternalCache>(Reuse.Singleton);
            mContainer.Register<IMgPipeline,GLGraphicsPipeline>(Reuse.Singleton);
            mContainer.Register<IGLGraphicsPipelineCompiler, MockGLGraphicsPipelineCompiler>(Reuse.Singleton);
            mContainer.Register<IGLGraphicsPipelineEntrypoint, MockGLGraphicsPipelineEntrypoint>(Reuse.Singleton);

            queue = mContainer.Resolve<IMgQueue>();
            var cmdEncoder = mContainer.Resolve<GLCmdCommandEncoder>();
            cmdBuf = new Magnesium.OpenGL.Internals.GLCmdCommandBuffer(true, cmdEncoder);

            framebuffer = new Magnesium.OpenGL.Internals.GLFramebuffer();
            renderpass = new GLRenderPass(new MgAttachmentDescription[] { });

            vertexBuffer = new MockGLBuffer
            {
                BufferId = 1,
                Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT,
            };
            indexBuffer = new MockGLBuffer
            {
                BufferId = 2,
                Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
            };

            var layout = new MockGLPipelineLayout
            {

            };
            mContainer.UseInstance<IGLPipelineLayout>(layout);

            var blockEntries = new GLUniformBlockEntry[]
            {

            };
            var arrayMapper = new Magnesium.OpenGL.Internals.GLInternalCacheArrayMapper(layout, blockEntries);

            mContainer.UseInstance<GLInternalCacheArrayMapper>(arrayMapper);

            var entrypoint = mContainer.Resolve<IGLGraphicsPipelineEntrypoint>();
            var internalCache = mContainer.Resolve<GLInternalCache>();
            var info = new MgGraphicsPipelineCreateInfo
            {
                VertexInputState = new MgPipelineVertexInputStateCreateInfo
                {
                    VertexAttributeDescriptions = new[]
                    {
                        new MgVertexInputAttributeDescription
                        {
                            Binding = 0,
                            Location = 0,
                            Format = MgFormat.R8G8B8A8_SNORM,
                        }
                    },
                    VertexBindingDescriptions = new[]
                    {
                        new MgVertexInputBindingDescription
                        {
                            Binding = 0,
                            InputRate = MgVertexInputRate.VERTEX,
                            Stride = 32,
                        }
                    }
                },
                RasterizationState = new MgPipelineRasterizationStateCreateInfo
                {
                    PolygonMode = MgPolygonMode.FILL,
                },
                InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo
                {
                    Topology = MgPrimitiveTopology.TRIANGLE_LIST,
                }
            };
            pipeline = new Magnesium.OpenGL.Internals.GLGraphicsPipeline(entrypoint, 1, info, internalCache, layout);


            var stateRenderer = mContainer.Resolve<IGLCmdStateRenderer>();
            stateRenderer.Initialize();
        }

        [TearDown]
        public void Cleanup()
        {
            mContainer.Dispose();
        }

        #region Mock test classes

        internal class MockGLBlitOperationEntrypoint : IGLBlitOperationEntrypoint
        {
            public void CopyBuffer(uint src, uint dst, IntPtr readOffset, IntPtr writeOffset, int size)
            {

            }
        }

        internal class MockGLSemaphoreEntrypoint : IGLSemaphoreEntrypoint
        {
            public IGLSemaphore CreateSemaphore()
            {
                throw new NotImplementedException();
            }
        }

        internal class MockGLPipelineLayout : IGLPipelineLayout
        {
            public GLUniformBinding[] Bindings
            {
                get;
                set;
            }

            public int NoOfBindingPoints
            {
                get;
                set;
            }

            public uint NoOfExpectedDynamicOffsets
            {
                get;
                set;
            }

            public uint NoOfStorageBuffers
            {
                get;
                set;
            }

            public GLDynamicOffsetInfo[] OffsetDestinations
            {
                get;
                set;
            }

            public IDictionary<int, GLBindingPointOffsetInfo> Ranges
            {
                get;
                set;
            }

            public void DestroyPipelineLayout(IMgDevice device, IMgAllocationCallbacks allocator)
            {

            }

            public bool Equals(IGLPipelineLayout other)
            {
                return ReferenceEquals(this, other);
            }
        }

        internal class MockGLGraphicsPipelineEntrypoint : IGLGraphicsPipelineEntrypoint
        {
            public void AttachShaderToProgram(int programID, int shader)
            {
                throw new NotImplementedException();
            }

            public bool CheckUniformLocation(int programId, int location)
            {
                throw new NotImplementedException();
            }

            public void CompileProgram(int programID)
            {
                throw new NotImplementedException();
            }

            public int CreateProgram()
            {
                throw new NotImplementedException();
            }

            public void DeleteProgram(int programID)
            {
                throw new NotImplementedException();
            }

            public int GetActiveUniforms(int programId)
            {
                throw new NotImplementedException();
            }

            public string GetCompilerMessages(int programID)
            {
                throw new NotImplementedException();
            }

            public bool HasCompilerMessages(int programID)
            {
                throw new NotImplementedException();
            }

            public bool IsCompiled(int programID)
            {
                throw new NotImplementedException();
            }
        }

        internal class MockGLGraphicsPipelineCompiler : IGLGraphicsPipelineCompiler
        {
            public int Compile(MgGraphicsPipelineCreateInfo info)
            {
                throw new NotImplementedException();
            }

            public GLUniformBlockEntry[] Inspect(int programId)
            {
                throw new NotImplementedException();
            }
        }

        internal class MockGLErrorHandler : IGLErrorHandler
        {
            public void CheckGLError()
            {

            }

            public void LogGLError(string location)
            {

            }

            public void Trace(string message)
            {

            }
        }

        internal class MockGLCmdBlendEntrypoint : IGLCmdBlendEntrypoint
        {
            public void ApplyBlendSeparateFunction(uint index, MgBlendFactor colorSource, MgBlendFactor colorDest, MgBlendFactor alphaSource, MgBlendFactor alphaDest)
            {

            }

            public void EnableBlending(uint index, bool value)
            {

            }

            public void EnableLogicOp(bool logicOpEnable)
            {

            }

            public GLGraphicsPipelineBlendColorState Initialize(uint noOfAttachments)
            {
                return new GLGraphicsPipelineBlendColorState
                {
                    Attachments = new GLGraphicsPipelineBlendColorAttachmentState[0],
                };

            }

            public bool IsEnabled(uint index)
            {
                throw new NotImplementedException();
            }

            public void LogicOp(MgLogicOp logicOp)
            {

            }

            public void SetBlendConstants(MgColor4f blendConstants)
            {

            }

            public void SetColorMask(uint index, MgColorComponentFlagBits colorMask)
            {

            }
        }

        internal class MockGLBuffer : IGLBuffer
        {
            public uint BufferId
            {
                get;
                set;
            }

            public bool IsBufferType
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public ulong RequestedSize
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IntPtr Source
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public MgBufferUsageFlagBits Usage
            {
                get;
                set;
            }

            public MgResult BindBufferMemory(IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
            {
                throw new NotImplementedException();
            }

            public void DestroyBuffer(IMgDevice device, IMgAllocationCallbacks allocator)
            {
                throw new NotImplementedException();
            }

            MgResult IMgBuffer.BindBufferMemory(IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
            {
                throw new NotImplementedException();
            }
        }

        internal class MockGLCmdClearEntrypoint : IGLCmdClearEntrypoint
        {
            public void ClearBuffers(GLQueueClearBufferMask combinedMask)
            {
                throw new NotImplementedException();
            }

            public GLClearValueState Initialize()
            {
                return new GLClearValueState();
            }

            public void SetClearColor(MgColor4f clearValue)
            {
                throw new NotImplementedException();
            }

            public void SetClearDepthValue(float value)
            {
                throw new NotImplementedException();
            }

            public void SetClearStencilValue(uint stencil)
            {
                throw new NotImplementedException();
            }
        }

        internal class MockGLCmdDepthEntrypoint : IGLCmdDepthEntrypoint
        {
            public bool IsDepthBufferEnabled
            {
                get
                {
                    return true;
                }
            }

            public void DisableDepthBuffer()
            {

            }

            public void EnableDepthBuffer()
            {

            }

            public GLGraphicsPipelineDepthState GetDefaultEnums()
            {
                return new GLGraphicsPipelineDepthState();
            }

            public GLGraphicsPipelineDepthState Initialize()
            {
                return new GLGraphicsPipelineDepthState();
            }

            public void SetClipControl(bool usingLowerLeftCorner, bool zeroToOneRange)
            {

            }

            public void SetDepthBounds(float min, float max)
            {

            }

            public void SetDepthBufferFunc(MgCompareOp func)
            {

            }

            public void SetDepthMask(bool isMaskOn)
            {

            }
        }

        internal class MockGLCmdVBOEntrypoint : IGLCmdVBOEntrypoint
        {
            public void AssociateBufferToLocation(uint vbo, uint location, uint bufferId, long offsets, uint stride)
            {

            }

            public void BindDoubleVertexAttribute(uint vbo, uint location, int size, GLVertexAttributeType pointerType, uint offset)
            {

            }

            public void BindFloatVertexAttribute(uint vbo, uint location, int size, GLVertexAttributeType pointerType, bool isNormalized, uint offset)
            {

            }

            public void BindIndexBuffer(uint vbo, uint bufferId)
            {

            }

            public void BindIntVertexAttribute(uint vbo, uint location, int size, GLVertexAttributeType pointerType, uint offset)
            {

            }

            public void DeleteVBO(uint vbo)
            {

            }

            public uint GenerateVBO()
            {
                return 1;
            }

            public void SetupVertexAttributeDivisor(uint vbo, uint location, uint divisor)
            {

            }
        }

        internal class MockGLCmdStencilEntrypoint : IGLCmdStencilEntrypoint
        {
            public bool IsStencilBufferEnabled
            {
                get
                {
                    return true;
                }
            }

            public void DisableStencilBuffer()
            {

            }

            public void EnableStencilBuffer()
            {

            }

            public GLGraphicsPipelineStencilState GetDefaultEnums()
            {
                return new GLGraphicsPipelineStencilState();
            }

            public GLQueueRendererStencilState Initialize()
            {
                return new GLQueueRendererStencilState
                {

                };
            }

            public void SetBackFaceCullStencilFunction(MgCompareOp func, int referenceStencil, uint compare)
            {

            }

            public void SetBackFaceStencilOperation(MgStencilOp stencilFail, MgStencilOp stencilDepthBufferFail, MgStencilOp stencilPass)
            {

            }

            public void SetFrontFaceCullStencilFunction(MgCompareOp func, int referenceStencil, uint compare)
            {

            }

            public void SetFrontFaceStencilOperation(MgStencilOp stencilFail, MgStencilOp stencilDepthBufferFail, MgStencilOp stencilPass)
            {

            }

            public void SetStencilFunction(MgCompareOp stencilFunction, int referenceStencil, uint compare)
            {

            }

            public void SetStencilOperation(MgStencilOp stencilFail, MgStencilOp stencilDepthBufferFail, MgStencilOp stencilPass)
            {

            }

            public void SetStencilWriteMask(MgStencilFaceFlagBits face, uint mask)
            {

            }
        }

        internal class MockGLCmdShaderProgramEntrypoint : IGLCmdShaderProgramEntrypoint
        {
            public void BindCombinedImageSampler(int programID, int binding, long value)
            {

            }

            public void BindProgram(int programID)
            {

            }

            public void BindStorageBuffer(uint binding, uint bufferId, IntPtr offset, IntPtr size)
            {

            }

            public void BindUniformBuffers(int count, uint[] buffers, IntPtr[] offsets, IntPtr[] sizes)
            {

            }

            public void BindVAO(uint vao)
            {

            }

            public void SetUniformBlock(int programID, int activeIndex, int bindingPoint)
            {

            }
        }

        internal class MockGLCmdScissorsEntrypoint : IGLCmdScissorsEntrypoint
        {
            public void ApplyScissors(GLCmdScissorParameter scissors)
            {

            }

            public void ApplyViewports(GLCmdViewportParameter viewports)
            {

            }
        }

        internal class MockGLCmdRasterizationEntrypoint : IGLCmdRasterizationEntrypoint
        {
            public bool CullingEnabled
            {
                get
                {
                    return true;
                }
            }

            public bool ScissorTestEnabled
            {
                get
                {
                    return true;
                }
            }

            public void DisableCulling()
            {

            }

            public void DisablePolygonOffset()
            {

            }

            public void DisableScissorTest()
            {

            }

            public void EnableCulling()
            {

            }

            public void EnablePolygonOffset(float slopeScaleDepthBias, float depthBias)
            {

            }

            public void EnableScissorTest()
            {

            }

            public GLRasterizerState Initialize()
            {
                return new GLRasterizerState();
            }

            public void SetCullingMode(bool front, bool back)
            {

            }

            public void SetLineWidth(float width)
            {

            }

            public void SetUsingCounterClockwiseWindings(bool b)
            {

            }
        }

        internal class MockGLCmdImageEntrypoint : IGLCmdImageEntrypoint
        {
            public void PerformOperation(GLCmdImageInstructionSet instructionSet)
            {

            }
        }

        internal class MockGLCmdDrawEntrypoint : IGLCmdDrawEntrypoint
        {
            public void DrawArrays(MgPrimitiveTopology topology, uint first, uint count, uint instanceCount, uint firstInstance)
            {

            }

            public void DrawArraysIndirect(MgPrimitiveTopology topology, IntPtr indirect, uint count, uint stride)
            {

            }

            public void DrawIndexed(MgPrimitiveTopology topology, MgIndexType indexType, uint first, uint count, uint instanceCount, int vertexOffset)
            {

            }

            public void DrawIndexedIndirect(MgPrimitiveTopology topology, MgIndexType indexType, IntPtr indirect, uint count, uint stride)
            {

            }
        }

        #endregion
    }
}
