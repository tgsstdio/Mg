using System;

namespace Magnesium
{
    // SUPER CLASS of graphics device (window-out) and offscreen (fbo)
    // for coding a pipeline around 
    // TODO : rename this class     
    public interface IMgEffectFramework : IDisposable
    {
        MgRect2D Scissor
        {
            get;
        }

        MgViewport Viewport
        {
            get;
        }

        MgRenderPassCreateInfo RenderpassInfo { get; }

        IMgRenderPass Renderpass
        {
            get;
        }

        IMgFramebuffer[] Framebuffers
        {
            get;
        }
    }
}

