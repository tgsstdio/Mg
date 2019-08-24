using Magnesium.Toolkit;
using OpenTK;

namespace Magnesium.PresentationSurfaces.OpenTK
{
    public class DesktopGLPresentationSurface : IMgPresentationSurface
    {
        private readonly INativeWindow mWindow;

        public IMgSurfaceKHR Surface
        {
            get
            {
                // DO NOT USE SURFACE
                return null;
            }
        }

        public DesktopGLPresentationSurface(INativeWindow window)
        {
            mWindow = window;
        }

        public void Dispose()
        {
            
        }

        public void Initialize(uint width, uint height)
        {
            // ADJUST WINDOW SIZE AS REQUESTED
            mWindow.ClientRectangle = new System.Drawing.Rectangle
                (mWindow.ClientRectangle.X, mWindow.ClientRectangle.Y, (int)width, (int)height);
        }
    }
}
