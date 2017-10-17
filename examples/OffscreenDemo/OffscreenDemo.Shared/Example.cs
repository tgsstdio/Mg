using Magnesium;
using System;
using System.Diagnostics;

namespace OffscreenDemo
{
    public class Example : IDisposable
    {
        private MgGraphicsConfigurationManager mManager;
        private IDemoApplication mApp;
        public Example(MgGraphicsConfigurationManager manager, IDemoApplication app)
        {
            mManager = manager;
            mApp = app;

            var createInfo = mApp.Initialize();

            mManager.Initialize(createInfo);
            Prepare();
        }

        void Prepare()
        {
            mApp.Prepare(mManager.Configuration, mManager.Graphics);
            mPrepared = true;
        }

        private bool mPrepared = false;
        public void Render()
        {
            if (!mPrepared)
                return;

            mApp.Update();
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

            if (disposing)
            {
                mApp.ReleaseManagedResources();
            }

            ReleaseUnmanagedResources();

            mIsDisposed = true;
        }

        private void ReleaseUnmanagedResources()
        {
            mApp.ReleaseUnmanagedResources();
            mManager.Dispose();
        }

        private void Draw()
        {
            var layerNo = mManager.Layer.BeginDraw(mManager.PostPresentCommand, null);

            //// Command buffer to be sumitted to the queue

            //var submitInfos = new[]
            //{
            //    new MgSubmitInfo
            //    {
            //        // ADD COMMANDS HERE
            //        CommandBuffers = null,
            //    }
            //};

            //// Submit to queue
            //var err = mManager.Configuration.Queue.QueueSubmit(submitInfos, null);
            //Debug.Assert(err == Result.SUCCESS);

            var signals = mApp.Render(mManager.Configuration.Queue, layerNo);

            //VulkanExampleBase::submitFrame();
            mManager.Layer.EndDraw(new[] { layerNo }, mManager.PrePresentCommand, signals);
        }
    }
}
