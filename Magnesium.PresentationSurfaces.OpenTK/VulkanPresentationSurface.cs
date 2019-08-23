using System.Diagnostics;
using Magnesium.Toolkit;
using OpenTK;

namespace Magnesium.PresentationSurfaces.OpenTK
{
    // HANDLES THE WINDOWING ISSUE
    public class VulkanPresentationSurface : IMgPresentationSurface
	{
		private readonly MgDriverContext mDriverContext;
		private readonly INativeWindow mWindow;
		public VulkanPresentationSurface(MgDriverContext context, INativeWindow window)
		{
			mDriverContext = context;
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
            mWindow.ClientRectangle  = new System.Drawing.Rectangle
                (mWindow.ClientRectangle.X, mWindow.ClientRectangle.Y, (int) width, (int) height);          

            var createInfo = new MgWin32SurfaceCreateInfoKHR {
				// DOUBLE CHECK 
				Hinstance = Process.GetCurrentProcess ().Handle,
				Hwnd = mWindow.WindowInfo.Handle,
			};
			var err = mDriverContext.Instance.CreateWin32SurfaceKHR (createInfo, null, out mSurface);
			Debug.Assert (err == MgResult.SUCCESS);
		}

		#endregion

		#region IDisposable implementation
		private bool mIsDisposed = false;
        public void Dispose ()
		{
			if (mIsDisposed)
				return;

			if (mSurface != null)
			{
				mSurface.DestroySurfaceKHR (mDriverContext.Instance, null);
			}

			mIsDisposed = true;
		}

		#endregion
	}
}

