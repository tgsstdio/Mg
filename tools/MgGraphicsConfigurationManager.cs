
namespace Magnesium
{
public class MgGraphicsConfigurationManager
{
        private IMgGraphicsConfiguration mConfiguration;
        private IMgGraphicsDevice mGraphicsDevice;
        private IMgPresentationLayer mPresentationLayer;
		private IMgSwapchainCollection mSwapchains;

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
		
            var createInfo = new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Color = MgFormat.R8G8B8A8_UINT,
                DepthStencil = MgFormat.D24_UNORM_S8_UINT,
                Width = mWidth,
                Height = mHeight,
            };

				

        public void Initialize(MgGraphicsDeviceCreateInfo createInfo)
        {
			mWidth = createInfo.Width;
			mHeight = createInfo.Height;
		
			mConfiguration.Initialize(mWidth, mHeight);	
		
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

		public void Dispose()
		{
			if (mPresentationLayer != null)
			{
				mPresentationLayer.Dispose();
			}
			
			if (mGraphicsDevice != null)
			{
				mGraphicsDevice.Dispose();
			}
			
			if (mSwapchains != null)
			{
				mSwapchains.Dispose();
			}
			
			//if (mConfiguration != null)
	//		{
	//				mConfiguration.Dispose();
			//}
		}
	}
}