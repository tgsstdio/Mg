﻿using Magnesium.OpenGL.DesktopGL.Internals;
using Magnesium.Toolkit;
using OpenTK;

namespace Magnesium.OpenGL.DesktopGL
{
    public class DesktopGLGraphicsContext : IMgGraphicsDeviceContext
    {
        INativeWindow mWindow;
        IGLExtensionLookup mExtensions;
        IMgGraphicsDeviceLogger mLogger;
        IGLFramebufferHelperSelector mSelector;
        IGLRenderer mQueueRenderer;
        private IGLBackbufferContext mBBContext;

        private IMgImageView mDepthStencilImageView;

        public DesktopGLGraphicsContext(
            INativeWindow window,
            IGLFramebufferHelperSelector selector,
            IGLExtensionLookup extensions,
            IMgGraphicsDeviceLogger logger,
            IGLRenderer queueRenderer,
            IGLBackbufferContext bbContext)
        {
            mWindow = window;
            mExtensions = extensions;
            mLogger = logger;
            mSelector = selector;
            mQueueRenderer = queueRenderer;
            mBBContext = bbContext;
        }

        public void Initialize(MgGraphicsDeviceCreateInfo createInfo)
        {

        }

        public void ReleaseDepthStencil()
        {
            mDepthStencilImageView = null;
        }

        public void SetupContext(MgGraphicsDeviceCreateInfo createInfo, MgFormat colorPassFormat, MgFormat depthPassFormat)
        {
            mBBContext.SetupContext(mWindow.WindowInfo, createInfo, colorPassFormat, depthPassFormat);

            mExtensions.Initialize();
            //mCapabilities.Initialize ();
            //mGLPlatform.Initialize ();
            mSelector.Initialize();
            mQueueRenderer.Initialize();
        }

        public IMgImageView SetupDepthStencil(MgGraphicsDeviceCreateInfo createInfo, IMgCommandBuffer setupCmdBuffer, MgFormat depthFormat)
        {
            mDepthStencilImageView = new GLNullDepthStencilImageView
            {
                Format = depthFormat,
                Width = createInfo.Width,
                Height = createInfo.Height,
            };
            return mDepthStencilImageView;
        }
    }
}

