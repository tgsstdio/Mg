using Magnesium;
using System.Diagnostics;
using Magnesium.Utilities;
using System.Runtime.InteropServices;
using System;

namespace OffscreenDemo
{
    public class TriplePaneDemoApplication : IDemoApplication
    {
        private MgOffscreenDeviceFactory mFactory;
        private MgOptimizedStorageBuilder mBuilder;
        private IRenderToTexturePipelineMediaPath mRttPath;
        private IPostProcessingPassThruMediaPath mPassThruPath;

        public TriplePaneDemoApplication(
            MgOffscreenDeviceFactory factory,
            MgOptimizedStorageBuilder builder,
            IRenderToTexturePipelineMediaPath rttPath,
            IPostProcessingPassThruMediaPath passThru            
        )
        {
            mFactory = factory;
            mBuilder = builder;
            mRttPath = rttPath;
            mPassThruPath = passThru;
        }

        public MgGraphicsDeviceCreateInfo Initialize()
        {
            return new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Width = 720,
                Height = 480,
                MinDepth = 0f,
                MaxDepth = 1f,
            };
        }

        const int TEXTURE_SIZE = 256;
        private IMgOffscreenDeviceAttachment mColorAttachment;
        private IMgOffscreenDeviceAttachment mDepthAttachment;
        private IMgOffscreenDeviceAttachment mNormalAttachment;
        private IMgEffectFramework mOffscreen;

        class UnmanagedResources
        {
            public SimpleEffectPipeline RTT { get; set; }
            public SimpleEffectPipeline PassThru { get; internal set; }
            public SimpleRenderableShape Torus { get; internal set; }
            public SimpleRenderableShape Quads { get; internal set; }
            public IMgSemaphore FirstStage { get; internal set; }
            public IMgSemaphore[] SecondStage { get; internal set; }
            public SimpleRenderBlock Block { get; internal set; }

            public void DisposeAll(IMgDevice device)
            {
                if (Block != null)
                {
                    Block.Dispose();
                    Block = null;
                }

                if (SecondStage != null)
                {
                    foreach (var stage in SecondStage)
                    {
                        stage.DestroySemaphore(device, null);
                    }
                }

                if (FirstStage != null)
                {
                    FirstStage.DestroySemaphore(device, null);
                    FirstStage = null;
                }

                if (Torus != null)
                {
                    Torus.Dispose();
                    Torus = null;
                }

                if (Quads != null)
                {
                    Quads.Dispose();
                    Quads = null;
                }

                if (RTT != null)
                {
                   RTT.Dispose();
                }

                if (PassThru != null)
                {
                    PassThru.Dispose();
                }
            }
        }


        private UnmanagedResources mUnmanagedResources;
        public void Prepare(IMgGraphicsConfiguration configuration, IMgGraphicsDevice screen)
        {
            mColorAttachment = mFactory.CreateColorAttachment(MgFormat.R8G8B8A8_UNORM, TEXTURE_SIZE, TEXTURE_SIZE);

            mDepthAttachment = mFactory.CreateDepthStencilAttachment(MgFormat.D16_UNORM, TEXTURE_SIZE, TEXTURE_SIZE);

            mNormalAttachment = mFactory.CreateColorAttachment(MgFormat.R8G8B8A8_UNORM, TEXTURE_SIZE, TEXTURE_SIZE);

            var createInfo = new MgOffscreenDeviceCreateInfo
            {
                Width = TEXTURE_SIZE,
                Height = TEXTURE_SIZE,
                MinDepth = 0f,
                MaxDepth = 1f,
                ColorAttachments = new[]
                {
                    new MgOffscreenColorAttachmentInfo
                    {
                        Format = MgFormat.R8G8B8A8_UNORM,
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                        StoreOp = MgAttachmentStoreOp.STORE,
                        View = mColorAttachment.View,
                    },
                    new MgOffscreenColorAttachmentInfo
                    {
                        Format = MgFormat.R8G8B8A8_UNORM,
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                        StoreOp = MgAttachmentStoreOp.STORE,
                        View = mNormalAttachment.View,
                    },
                },
                DepthStencilAttachment = new MgOffscreenDepthStencilAttachmentInfo
                {
                    Format = MgFormat.D16_UNORM,
                    LoadOp = MgAttachmentLoadOp.CLEAR,
                    StoreOp = MgAttachmentStoreOp.STORE,
                    StencilLoadOp = MgAttachmentLoadOp.CLEAR,
                    StencilStoreOp = MgAttachmentStoreOp.STORE,
                    View = mDepthAttachment.View,
                    Layout = MgImageLayout.GENERAL,
                }
            };
            // Create a FBO and attach the textures
            mOffscreen = mFactory.CreateOffscreenDevice(createInfo);

            mUnmanagedResources = new UnmanagedResources();
            PrepareRtt(configuration);

            PreparePassThru(configuration, screen);

            var torusKnot = new TorusKnot(256, 32, 0.2, 7, 8, 1);

            var colorFormat = mOffscreen.RenderpassInfo.Attachments[0].Format;
            var torusItem = new RttRenderingElement(
                torusKnot,
                new MgClearValue[]
                {
                    MgClearValue.FromColorAndFormat(
                        mOffscreen.RenderpassInfo.Attachments[0].Format,
                        new MgColor4f(0.7f, 0f, 0f, 1f)
                    ),
                    MgClearValue.FromColorAndFormat(
                        mOffscreen.RenderpassInfo.Attachments[1].Format,
                        new MgColor4f(0f, 0.7f, 0.7f, 1f)
                    ),
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue(1.0f, 0) }
                });
            var torusModel = new SimpleRenderableShape(torusItem, mUnmanagedResources.RTT);

            var vertexData = new[]
            {
                new QuadVertexData
                {
                    TexCoord = new TkVector2(0f,1f),
                    Position = new TkVector2(-1f,1f),
                },
                new QuadVertexData
                {
                    TexCoord = new TkVector2(0f,0f),
                    Position = new TkVector2(-1f,-1f),
                },
                new QuadVertexData
                {
                    TexCoord = new TkVector2(1f,0f),
                    Position = new TkVector2(1f,-1f),
                },
                new QuadVertexData
                {
                    TexCoord = new TkVector2(1f,1f),
                    Position = new TkVector2(1f, 1f),
                },
            };
            var indices = new uint[] { 0, 1, 2, 0, 2, 3 };

            const float fieldOfView = 0.7853982f;
            float aspectRatio = screen.Viewport.Width / screen.Viewport.Height;
            var uniforms = new[]
            {
                new PostProcessUBO
                {
                    ProjectionMatrix =
                        TkMatrix4.CreatePerspectiveFieldOfView(
                          fieldOfView,
                          aspectRatio,
                          0.1f, 1000f),
                    ModelViewMatrix =
                        TkMatrix4.LookAt(
                            0f, 0f, 2.8f,
                            0f, 0f, 0f,
                            0f, 1f, 0f
                        ),
                    Offset = new TkVector4(-2.2f, 0f, 0f, 0f),
                }
            };

            var quads = new PostProcessQuad<QuadVertexData, uint, PostProcessUBO>(
                vertexData,
                indices,
                uniforms,
                mColorAttachment,
                new MgClearValue[]
                {
                    MgClearValue.FromColorAndFormat(
                        screen.RenderpassInfo.Attachments[0].Format,
                        new MgColor4f(0.1f, 0.2f, 0.3f, 1f)),
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue(1.0f, 0) },
                });
            var quadModel = new SimpleRenderableShape(quads, mUnmanagedResources.PassThru);
            var block = new SimpleRenderBlock(mBuilder, new[] { torusModel, quadModel } );
            block.Initialize(configuration);
            mUnmanagedResources.Block = block;

            mUnmanagedResources.Torus = torusModel;
            mUnmanagedResources.Quads = quadModel;

            torusKnot.Dispose();

            mUnmanagedResources.FirstStage = GenerateSemaphore(configuration);

            var noOfSemaphores = screen.Framebuffers.Length;
            mUnmanagedResources.SecondStage = new IMgSemaphore[noOfSemaphores];
            for (var i = 0; i < noOfSemaphores; i += 1)
            {
                mUnmanagedResources.SecondStage[i] = GenerateSemaphore(configuration);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PostProcessUBO
        {
            public TkMatrix4 ProjectionMatrix { get; internal set; }
            public TkMatrix4 ModelViewMatrix { get; internal set; }
            public TkVector4 Offset { get; set; }
        }

        private IMgSemaphore GenerateSemaphore(IMgGraphicsConfiguration configuration)
        {
            var createInfo = new MgSemaphoreCreateInfo
            {

            };
            var err = configuration.Device.CreateSemaphore(createInfo, null, out IMgSemaphore temp);
            Debug.Assert(err == Result.SUCCESS);
            return temp;
        }

        private void PreparePassThru(IMgGraphicsConfiguration configuration, IMgGraphicsDevice screen)
        {
            var seed = new PostProcessPassThruPipelineSeed(mPassThruPath);
            var passThru = new SimpleEffectPipeline(seed);
            passThru.Initialize(configuration, screen);
            mUnmanagedResources.PassThru = passThru;

        }

        private void PrepareRtt(IMgGraphicsConfiguration configuration)
        {
            var rttSeed = new RenderToTexturePipelineSeed(mRttPath);
            var renderToTexture = new SimpleEffectPipeline(rttSeed);
            renderToTexture.Initialize(configuration, mOffscreen);
            mUnmanagedResources.RTT = renderToTexture;
        }

        public void ReleaseManagedResources(IMgGraphicsConfiguration configuration)
        {

        }

        public void ReleaseUnmanagedResources(IMgGraphicsConfiguration configuration)
        {
            mOffscreen.Dispose();

            // Clean up what we allocated before exiting
            mColorAttachment.Dispose();

            mDepthAttachment.Dispose();

            mNormalAttachment.Dispose();

            if (mUnmanagedResources != null)
            {
                mUnmanagedResources.DisposeAll(configuration.Device);
            }
        }

        public IMgSemaphore[] Render(IMgQueue queue, uint layerNo, IMgSemaphore semaphore)
        {
            //// Command buffer to be sumitted to the queue

            var first = mUnmanagedResources.Torus.Order;
            var second = mUnmanagedResources.Quads.Order;

            IMgSemaphore isDone = mUnmanagedResources.SecondStage[layerNo];

            var firstInfo = new[]
            {
                new MgSubmitInfo
                {
                    // ADD COMMANDS HERE                    
                     WaitSemaphores = new []
                     {
                        new MgSubmitInfoWaitSemaphoreInfo
                        {
                            WaitDstStageMask = MgPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                            WaitSemaphore = semaphore,
                        },
                    },
                    CommandBuffers = new [] { first.CommandBuffers[0] },
                    // SignalSemaphores = new [] { mUnmanagedResources.FirstStage },
                    SignalSemaphores = new []
                    {
                        isDone
                    }
                },
            };

            var err = queue.QueueSubmit(firstInfo, null);
            Debug.Assert(err == Result.SUCCESS);

            //var secondInfo = new[] {
            //    new MgSubmitInfo
            //    {
            //        WaitSemaphores = new []
            //        {
            //            new MgSubmitInfoWaitSemaphoreInfo
            //            {
            //                WaitDstStageMask = MgPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
            //                WaitSemaphore = mUnmanagedResources.FirstStage,
            //            },
            //        },
            //        CommandBuffers = new [] { second.CommandBuffers[layerNo] },
            //        SignalSemaphores = new []
            //        {
            //            isDone
            //        }
            //    }
            //};

            ////// Submit to queue
            //err = queue.QueueSubmit(secondInfo, null);
            //Debug.Assert(err == Result.SUCCESS);

            err = queue.QueueWaitIdle();
            Debug.Assert(err == Result.SUCCESS);

            return new IMgSemaphore[] { isDone };
        }

        public void Update(IMgGraphicsConfiguration configuration)
        {

        }
    }
}
