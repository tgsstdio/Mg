using Magnesium;
using System;

namespace OffscreenDemo
{
    public interface IDemoApplication
    {
        MgGraphicsDeviceCreateInfo Initialize();

        void Prepare(IMgGraphicsConfiguration configuration, IMgGraphicsDevice screen);

        void Update();

        IMgSemaphore[] Render(IMgQueue queue, uint layerNo);

        void ReleaseManagedResources();
        void ReleaseUnmanagedResources();
    }
}
