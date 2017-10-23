using Magnesium;
using System;

namespace OffscreenDemo
{
    public interface IDemoApplication
    {
        MgGraphicsDeviceCreateInfo Initialize();

        void Prepare(IMgGraphicsConfiguration configuration, IMgGraphicsDevice screen);

        void Update(IMgGraphicsConfiguration configuration);

        IMgSemaphore[] Render(IMgQueue queue, uint layerNo, IMgSemaphore semaphore);

        void ReleaseManagedResources(IMgGraphicsConfiguration configuration);
        void ReleaseUnmanagedResources(IMgGraphicsConfiguration configuration);
    }
}
