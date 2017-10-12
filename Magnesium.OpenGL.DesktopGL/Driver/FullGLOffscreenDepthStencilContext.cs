using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLOffscreenDepthStencilContext : IMgGraphicsDeviceContext
    {
        public void Initialize(MgGraphicsDeviceCreateInfo createInfo)
        {
         
        }

        public void ReleaseDepthStencil()
        {

        }

        public void SetupContext(MgGraphicsDeviceCreateInfo createInfo, MgFormat colorPassFormat, MgFormat depthPassFormat)
        {
    
        }

        private GLNullColorImageView mDepthStencilView;
        public IMgImageView SetupDepthStencil(MgGraphicsDeviceCreateInfo createInfo, IMgCommandBuffer setupCmdBuffer, MgFormat depthFormat)
        {
            mDepthStencilView = new GLNullColorImageView
            {
                Format = depthFormat,
                Width = createInfo.Width,
                Height = createInfo.Height,
            };
            return mDepthStencilView;
        }
    }
}
