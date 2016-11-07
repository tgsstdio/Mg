using System.Diagnostics;
using Magnesium;
using OpenTK;
using System;

namespace Magnesium.Platform.OpenTK
{
    // HANDLES THE WINDOWING ISSUE
    public class MgOpenTKPresentationSurface : IMgPresentationSurface
	{
		private readonly MgDriverContext mDriver;
		private readonly INativeWindow mWindow;
		public MgOpenTKPresentationSurface(MgDriverContext driver, INativeWindow window)
		{
			mDriver = driver;
			mWindow = window;
		}

		#region IMgPresentationLayer implementation

		public IMgSurfaceKHR Surface {
			get {
				return mSurface;
			}
		}

		private IMgSurfaceKHR mSurface;
		public void Initialize (uint width, uint height)
		{
            /// SEEMS THE WINDOW DIMENSIONS (WIDTH, HEIGHT) MUST BE SET PROIR TO BEING PASSED INTO VULKAN
            /// DIMENSIONS TAKEN FROM IPresentationParameters
            mWindow.ClientRectangle = new System.Drawing.Rectangle
                (width, height, mPresentationParameters.BackBufferWidth, mPresentationParameters.BackBufferHeight);          

            var createInfo = new MgWin32SurfaceCreateInfoKHR {
				// DOUBLE CHECK 
				Hinstance = Progress.GetCurrentProcess ().Handle,
				Hwnd = mWindow.WindowInfo.Handle,
			};
			var err = mDriver.Instance.CreateWin32SurfaceKHR (createInfo, null, out mSurface);
			Debug.Assert (err == Result.SUCCESS);
		}

		#endregion

		#region IDisposable implementation
		private bool mIsDisposed = false;
        private IPresentationParameters mPresentationParameters;

        public void Dispose ()
		{
			if (mIsDisposed)
				return;

			if (mSurface != null)
			{
				mSurface.DestroySurfaceKHR (mDriver.Instance, null);
			}

			mIsDisposed = true;
		}

		#endregion
	}
}

