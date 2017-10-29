using Magnesium;
using System.Diagnostics;

namespace OffscreenDemo
{
    public class MgGraphicsConfigurationManager
    {        
        private IMgGraphicsConfiguration mConfiguration;
        public IMgGraphicsConfiguration Configuration {
            get
            {
                return mConfiguration;
            }
        }

        private IMgGraphicsDevice mGraphicsDevice;
        public IMgGraphicsDevice Graphics
        {
            get
            {
                return mGraphicsDevice;
            }
        }

        private IMgPresentationLayer mPresentationLayer;
        public IMgPresentationLayer Layer
        {
            get
            {
                return mPresentationLayer;
            }
        }

        private IMgSwapchainCollection mSwapchains;
        public IMgSwapchainCollection Swapchains
        {
            get
            {
                return mSwapchains;
            }
        }

        private uint mWidth;
        public uint Width
        {
            get
            {
                return mWidth;
            }
        }

        private uint mHeight;
        public uint Height
        {
            get
            {
                return mHeight;
            }
        }

        public MgGraphicsConfigurationManager(
            IMgGraphicsConfiguration configuration,
            IMgSwapchainCollection swapchains,
            IMgGraphicsDevice graphicsDevice,
            IMgPresentationLayer presentationLayer
        )
        {
            mConfiguration = configuration;
            mSwapchains = swapchains;
            mGraphicsDevice = graphicsDevice;
            mPresentationLayer = presentationLayer;
        }

        public void Initialize(MgGraphicsDeviceCreateInfo createInfo)
        {
            mWidth = createInfo.Width;
            mHeight = createInfo.Height;

            mConfiguration.Initialize(mWidth, mHeight);

            SetupGraphicsDevice(createInfo);
            SetupPresentationBarrierCommands();
            SetupSemaphores();
        }

        private void SetupSemaphores()
        {
            var semaphoreCreateInfo = new MgSemaphoreCreateInfo { };
            // Semaphore used to ensures that image presentation is complete before starting to submit again
            var err = mConfiguration.Device.CreateSemaphore(semaphoreCreateInfo, null, out IMgSemaphore pSemaphore);
            Debug.Assert(err == Result.SUCCESS);
            PresentComplete = pSemaphore;
        }

        private void SetupGraphicsDevice(MgGraphicsDeviceCreateInfo createInfo)
        {
            Debug.Assert(mConfiguration.Partition != null);

            const int NO_OF_BUFFERS = 1;
            var buffers = new IMgCommandBuffer[NO_OF_BUFFERS];
            var pAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = NO_OF_BUFFERS,
                CommandPool = mConfiguration.Partition.CommandPool,
                Level = MgCommandBufferLevel.PRIMARY,
            };

            mConfiguration.Device.AllocateCommandBuffers(pAllocateInfo, buffers);


            var setupCmdBuffer = buffers[0];

            var cmdBufInfo = new MgCommandBufferBeginInfo
            {

            };


            var err = setupCmdBuffer.BeginCommandBuffer(cmdBufInfo);
            Debug.Assert(err == Result.SUCCESS);

            mGraphicsDevice.Create(setupCmdBuffer, mSwapchains, createInfo);

            err = setupCmdBuffer.EndCommandBuffer();
            Debug.Assert(err == Result.SUCCESS);


            var submission = new[] {
                new MgSubmitInfo
                {
                    CommandBuffers = new IMgCommandBuffer[]
                    {
                        buffers[0],
                    },
                }
            };

            err = mConfiguration.Queue.QueueSubmit(submission, null);
            Debug.Assert(err == Result.SUCCESS);

            mConfiguration.Queue.QueueWaitIdle();

            mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, buffers);
        }

        IMgCommandBuffer[] mPresentBuffers;
        public IMgCommandBuffer PrePresentCommand
        {
            get
            {
                Debug.Assert(mPresentBuffers != null);
                return mPresentBuffers[0];
            }
                    }


        public IMgCommandBuffer PostPresentCommand
        {
            get
            {
                Debug.Assert(mPresentBuffers != null);
                return mPresentBuffers[1];
            }
        }

        public IMgSemaphore PresentComplete { get; private set; }

        // Command buffers for submitting present barriers
        private void SetupPresentationBarrierCommands()
        {
            var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = 2,
                CommandPool = mConfiguration.Partition.CommandPool,
                Level = MgCommandBufferLevel.PRIMARY,
            };

            mPresentBuffers = new IMgCommandBuffer[2];

            var device = mConfiguration.Device;
            Debug.Assert(device != null);

            var err = device.AllocateCommandBuffers(cmdBufAllocateInfo, mPresentBuffers);
            Debug.Assert(err == Result.SUCCESS);

            // Pre present
            //PrePresentCommand = mPresentBuffers[0];

            // Post present
            //PostPresentCommand = mPresentBuffers[1];
        }

        public void Dispose()
        {

            if (mGraphicsDevice != null)
            {
                mGraphicsDevice.Dispose();
            }

            if (mSwapchains != null)
            {
                mSwapchains.Dispose();
            }

            if (mConfiguration != null)
            {
                if (PresentComplete != null)
                    PresentComplete.DestroySemaphore(mConfiguration.Device, null);

                if (mConfiguration.Partition != null)
                {
                    var commandPool = mConfiguration.Partition.CommandPool;
                    if (commandPool != null)
                    {
                        if (mPresentBuffers != null)
                        {
                            mConfiguration.Device.FreeCommandBuffers(
                                commandPool,
                                mPresentBuffers);
                            mPresentBuffers = null;
                        }
                    }
                }

                mConfiguration.Dispose();
            }

        }
    }
}
    

