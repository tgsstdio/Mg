using OpenTK.Graphics;
using OpenTK.Platform;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public interface IBackbufferContext : IDisposable
    {
        IGraphicsContext Context { get; }
        void SetupContext(IWindowInfo wnd, MgGraphicsDeviceCreateInfo createInfo);
    }
}
