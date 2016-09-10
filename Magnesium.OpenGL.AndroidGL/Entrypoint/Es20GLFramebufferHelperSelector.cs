using System;

namespace Magnesium.OpenGL.AndroidGL
{
	public class Es20GLFramebufferHelperSelector : IGLFramebufferHelperSelector
	{
		private readonly IGLFramebufferSupport mCapabilities;
		private readonly IGLExtensionLookup mLookup;
		private readonly IGLErrorHandler mErrHandler;
		public Es20GLFramebufferHelperSelector  (IGLFramebufferSupport capabilities, IGLExtensionLookup lookup, IGLErrorHandler errHandler)
		{
			mCapabilities = capabilities;
			mLookup = lookup;
			mErrHandler = errHandler;
		}

		#region IGLFramebufferHelperSelector implementation

		public void Initialize ()
		{
			if (mCapabilities.SupportsFramebufferObjectARB())
			{
				Helper = new AndroidGLFramebufferHelper(mLookup, mErrHandler);
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

