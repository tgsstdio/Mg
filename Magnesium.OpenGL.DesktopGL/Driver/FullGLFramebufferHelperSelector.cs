using System;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLFramebufferHelperSelector : IGLFramebufferHelperSelector
	{
		private readonly IGLFramebufferSupport mCapabilities;
		readonly IGLErrorHandler mErrHandler;

		public FullGLFramebufferHelperSelector (IGLFramebufferSupport capabilities, IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
			mCapabilities = capabilities;
		}

		#region IGLFramebufferHelperSelector implementation

		public void Initialize ()
		{
			if (mCapabilities.SupportsFramebufferObjectARB())
			{
				Helper = new FullGLFramebufferHelper (mErrHandler);
			}
			//#if !(GLES || MONOMAC)
			else if (mCapabilities.SupportsFramebufferObjectEXT())
			{
				Helper = new FullGLFramebufferHelperEXT(mErrHandler);
			}
			//#endif
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

