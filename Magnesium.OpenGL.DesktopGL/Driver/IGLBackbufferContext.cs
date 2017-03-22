using OpenTK.Graphics;
using OpenTK.Platform;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public interface IGLBackbufferContext : IDisposable
    {
        IGraphicsContext Context { get; }
        void SetupContext(IWindowInfo wnd, MgFormat colorPassFormat, MgFormat depthPassFormat, MgGraphicsDeviceCreateInfo createInfo);
    }
}
