using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLFramebufferSupport : IGLFramebufferSupport
	{
		private readonly IGLExtensionLookup mExtensions;
		public FullGLFramebufferSupport (IGLExtensionLookup extensions)
		{
			mExtensions = extensions;
		}

		#region IGraphicsCapabilitiesLookup implementation

		public bool SupportsFramebufferObjectARB ()
		{
			return mExtensions.HasExtension("GL_ARB_framebuffer_object");
		}

		public bool SupportsFramebufferObjectEXT ()
		{
			return mExtensions.HasExtension("GL_EXT_framebuffer_object");
		}

		#endregion
	}
}

