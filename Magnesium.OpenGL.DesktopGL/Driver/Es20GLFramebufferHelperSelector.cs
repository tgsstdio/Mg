using System;

namespace Magnesium.OpenGL.DesktopGL
{
	public class Es20GLFramebufferHelperSelector : IGLFramebufferHelperSelector
	{
		private readonly IGLFramebufferSupport mCapabilities;
		readonly IGLErrorHandler mErrHandler;
		
		public Es20GLFramebufferHelperSelector  (IGLFramebufferSupport capabilities, IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
			mCapabilities = capabilities;
		}

		#region IGLFramebufferHelperSelector implementation

		public void Initialize ()
		{
			if (mCapabilities.SupportsFramebufferObjectARB())
			{
				Helper = new Es20GLFramebufferHelper(mErrHandler);
			}
			else
			{
				throw new PlatformNotSupportedException(
					"MonoGame requires either ARB_framebuffer_object or EXT_framebuffer_object." +
					"Try updating your graphics drivers.");
			}
		}

		public IGLFramebufferHelper Helper {
			get;
			private set;
		}

		#endregion
	}
}

