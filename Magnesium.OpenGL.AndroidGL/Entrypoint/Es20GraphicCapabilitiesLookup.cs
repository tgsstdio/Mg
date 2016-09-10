using Magnesium.OpenGL;

namespace Magnesium.OpenGL.AndroidGL
{
	public class Es20GraphicCapabilitiesLookup : IGLFramebufferSupport
	{
		#region IGraphicsCapabilitiesLookup implementation

		public bool SupportsFramebufferObjectARB ()
		{
			return true;
		}

		public bool SupportsFramebufferObjectEXT ()
		{
			return false;
		}

		#endregion
	}
}

