using OpenTK.Graphics;
using OpenTK.Platform;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class OpenTKBackbufferContext : IBackbufferContext
    {
        private IMgGraphicsDeviceLogger mLogger;
        public OpenTKBackbufferContext(IMgGraphicsDeviceLogger logger)
        {
            mLogger = logger;
        }

        private ColorFormat GetColorFormat(Magnesium.MgFormat format)
        {
            switch (format)
            {
                case MgFormat.R8_UNORM:
                case MgFormat.R8_SINT:
                case MgFormat.R8_UINT:
                    return new ColorFormat(8, 0, 0, 0);
                case MgFormat.R8G8_UNORM:
                case MgFormat.R8G8_SINT:
                case MgFormat.R8G8_UINT:
                    return new ColorFormat(8, 8, 0, 0);

                case MgFormat.B5G6R5_UNORM_PACK16:
                    return new ColorFormat(5, 6, 5, 0);
                case MgFormat.B4G4R4A4_UNORM_PACK16:
                    return new ColorFormat(4, 4, 4, 4);
                case MgFormat.B5G5R5A1_UNORM_PACK16:
                    return new ColorFormat(5, 5, 5, 1);

                case MgFormat.B8G8R8_UINT:
                case MgFormat.B8G8R8_SINT:
                case MgFormat.R8G8B8_UINT:
                case MgFormat.R8G8B8_SINT:
                case MgFormat.R8G8B8_SRGB:
                case MgFormat.B8G8R8_SRGB:
                case MgFormat.B8G8R8_UNORM:
                case MgFormat.R8G8B8_UNORM:
                    return new ColorFormat(8, 8, 8, 0);

                case MgFormat.B8G8R8A8_UNORM:
                case MgFormat.R8G8B8A8_UNORM:
                case MgFormat.R8G8B8A8_SRGB:
                case MgFormat.B8G8R8A8_SRGB:
                case MgFormat.R8G8B8A8_UINT:
                case MgFormat.R8G8B8A8_SINT:
                case MgFormat.B8G8R8A8_UINT:
                case MgFormat.B8G8R8A8_SINT:
                    return new ColorFormat(8, 8, 8, 8);

                case MgFormat.A2B10G10R10_UNORM_PACK32:
                    return new ColorFormat(10, 10, 10, 2);

                default:
                    // Floating point backbuffers formats could be implemented
                    // but they are not typically used on the backbuffer. In
                    // those cases it is better to create a render target instead.
                    throw new NotSupportedException();
            }
        }

        int GetDepthBit(MgFormat format)
        {
            switch (format)
            {
                case MgFormat.D16_UNORM:
                case MgFormat.D16_UNORM_S8_UINT:
                    return 16;
                case MgFormat.D24_UNORM_S8_UINT:
                    return 24;
                case MgFormat.D32_SFLOAT:
                case MgFormat.D32_SFLOAT_S8_UINT:
                    return 32;
                default:
                    throw new NotSupportedException();
            }
        }

        int GetStencilBit(MgFormat format)
        {
            switch (format)
            {
                case MgFormat.D16_UNORM:
                case MgFormat.D32_SFLOAT:
                    return 0;
                case MgFormat.D16_UNORM_S8_UINT:
                case MgFormat.D24_UNORM_S8_UINT:
                case MgFormat.D32_SFLOAT_S8_UINT:
                    return 8;
                default:
                    throw new NotSupportedException();
            }
        }

        public IGraphicsContext Context { get; private set; }

        public void SetupContext(IWindowInfo wnd, MgGraphicsDeviceCreateInfo createInfo)
        {
            if (Context != null)
                Context.Dispose();

            GraphicsMode mode;
            // Create an OpenGL compatibility context
            var flags = GraphicsContextFlags.Default;
            int major = 1;
            int minor = 0;
            if (Context == null || Context.IsDisposed)
            {
                var color = GetColorFormat(createInfo.Color);
                var depthBit = GetDepthBit(createInfo.DepthStencil);
                var stencilBit = GetStencilBit(createInfo.DepthStencil);
                var samples = (int)createInfo.Samples;
                if (samples == 0)
                {
                    // Use a default of 4x samples if PreferMultiSampling is enabled
                    // without explicitly setting the desired MultiSampleCount.
                    samples = 4;
                }
                mode = new GraphicsMode(color, depthBit, stencilBit, samples);
                try
                {
                    Context = new GraphicsContext(mode, wnd, major, minor, flags);
                }
                catch (Exception e)
                {
                    mLogger.Log(string.Format("Failed to create OpenGL context, retrying. Error: {0}", e));
                    major = 1;
                    minor = 0;
                    flags = GraphicsContextFlags.Default;
                    Context = new GraphicsContext(mode, wnd, major, minor, flags);
                }
            }
            Context.MakeCurrent(wnd);
            (Context as IGraphicsContextInternal).LoadAll();
            //Context.SwapInterval = mDeviceQuery.GetSwapInterval (mPresentation.PresentationInterval);
            // TODO : background threading 
            // Provide the graphics context for background loading
            // Note: this context should use the same GraphicsMode,
            // major, minor version and flags parameters as the main
            // context. Otherwise, context sharing will very likely fail.
            //			if (Threading.BackgroundContext == null)
            //			{
            //				Threading.BackgroundContext = new GraphicsContext(mode, wnd, major, minor, flags);
            //				Threading.WindowInfo = wnd;
            //				Threading.BackgroundContext.MakeCurrent(null);
            //			}
            Context.MakeCurrent(wnd);
        }

        ~OpenTKBackbufferContext()
        {
            Dispose(false);
        }

        private bool mIsDisposed = true;
        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;

            if (Context != null)
                Context.Dispose();

            mIsDisposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }    
    }
}
