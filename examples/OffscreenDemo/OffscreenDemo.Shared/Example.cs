using Magnesium;
using System;
using System.Diagnostics;

namespace OffscreenDemo
{
    public class Example : IDisposable
    {
        private MgGraphicsConfigurationManager mManager;
        public Example(MgGraphicsConfigurationManager manager)
        {
            mManager = manager;

            var createInfo = new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Width = 1280,
                Height = 720,
            };

            mManager.Initialize(createInfo);
            Prepare();
        }

        void Prepare()
        {
            // ADD STUFF HERE

            mPrepared = true;
        }

        private bool mPrepared = false;
        public void Render()
        {
            if (!mPrepared)
                return;
            // updateUniformBuffers();
            Draw();
        }

        ~Example()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool mIsDisposed = false;
        private void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;

            ReleaseUnmanagedResources();

            mIsDisposed = true;
        }

        private void ReleaseUnmanagedResources()
        {
            mManager.Dispose();
        }

        private void Draw()
        {
            var layerNo = mManager.Layer.BeginDraw(mManager.PostPresentCommand, null);

            // Command buffer to be sumitted to the queue

            var submitInfos = new[]
            {
                new MgSubmitInfo
                {
                    // ADD COMMANDS HERE
                    CommandBuffers = null,
                }
            };

            // Submit to queue
            var err = mManager.Configuration.Queue.QueueSubmit(submitInfos, null);
            Debug.Assert(err == Result.SUCCESS);

            //VulkanExampleBase::submitFrame();
            mManager.Layer.EndDraw(new[] { layerNo }, mManager.PrePresentCommand, null);
        }
    }
}
