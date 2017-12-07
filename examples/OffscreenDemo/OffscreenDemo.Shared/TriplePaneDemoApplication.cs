using System;
using System.Collections.Generic;
using System.Text;
using Magnesium;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Magnesium.Utilities;

namespace OffscreenDemo
{
    partial class TriplePaneDemoApplication : IDemoApplication
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

            public void DisposeAll(IMgDevice device)
            {

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
            var torusItem = new MgIsolatedRenderingElement(
                torusKnot,
                new MgClearValue[]
                {
                    MgClearValue.FromColorAndFormat(colorFormat, new MgColor4f(0.1f, 0.2f, 0.3f, 1f)),
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue(1.0f, 0) },
                });
            var torusModel = new SimpleRenderableShape(torusItem, mUnmanagedResources.RTT);
            var quads = new PostProcessQuad();
            var quadModel = new SimpleRenderableShape(quads, mUnmanagedResources.PassThru);
            var block = new SimpleRenderBlock(mBuilder, new[] { torusModel, quadModel } ));

            throw new NotImplementedException();
        }

        private static IMgCommandPool CreateCommandPool(IMgGraphicsConfiguration configuration)
        {
            var poolCreateInfo = new MgCommandPoolCreateInfo { };
            var err = configuration.Device.CreateCommandPool(poolCreateInfo, null, out IMgCommandPool pool);
            Debug.Assert(err == Result.SUCCESS);
            return pool;
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

            var first = mUnmanagedResources.Orders[0];
            var second = mUnmanagedResources.Orders[1];

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
                    SignalSemaphores = new [] { mUnmanagedResources.FirstStage },
                },
            };

            var err = queue.QueueSubmit(firstInfo, null);
            Debug.Assert(err == Result.SUCCESS);

            IMgSemaphore isDone = mUnmanagedResources.SecondStage[layerNo];

            var secondInfo = new[] {
                new MgSubmitInfo
                {
                    WaitSemaphores = new []
                    {
                        new MgSubmitInfoWaitSemaphoreInfo
                        {
                            WaitDstStageMask = MgPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                            WaitSemaphore = mUnmanagedResources.FirstStage,
                        },
                    },
                    CommandBuffers = new [] { second.CommandBuffers[layerNo] },
                    SignalSemaphores = new []
                    {
                        isDone
                    }
                }
            };

            //// Submit to queue
            err = queue.QueueSubmit(secondInfo, null);
            Debug.Assert(err == Result.SUCCESS);

            err = queue.QueueWaitIdle();
            Debug.Assert(err == Result.SUCCESS);

            return new IMgSemaphore[] { isDone };
        }

        public void Update(IMgGraphicsConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
