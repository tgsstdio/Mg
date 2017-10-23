using System;
using Magnesium;

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
        }

        public void Initialize()
        {
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

            mApp.Update(mManager.Configuration);
            RenderFrame();
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
                mApp.ReleaseManagedResources(mManager.Configuration);
            }

            ReleaseUnmanagedResources();

            mIsDisposed = true;
        }


        private void ReleaseUnmanagedResources()
        {
            mApp.ReleaseUnmanagedResources(mManager.Configuration);
            mManager.Dispose();
        }

        private void RenderFrame()
        {
            var layerNo = mManager.Layer.BeginDraw(mManager.PostPresentCommand, mManager.PresentComplete);

            var signals = mApp.Render(mManager.Configuration.Queue, layerNo, mManager.PresentComplete);

            mManager.Layer.EndDraw(new[] { layerNo }, mManager.PrePresentCommand, signals);
        }
    }
}
