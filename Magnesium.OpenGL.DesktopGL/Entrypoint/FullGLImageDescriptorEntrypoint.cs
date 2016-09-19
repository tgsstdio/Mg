using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLImageDescriptorEntrypoint : IGLImageDescriptorEntrypoint
	{
		readonly IGLErrorHandler mErrHandler;

		public FullGLImageDescriptorEntrypoint (IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
		}

		#region IGLImageDescriptorEntrypoint implementation

		public ulong CreateHandle (int textureId, int samplerId)
		{
			long texHandle = GL.Arb.GetTextureSamplerHandle (textureId, samplerId);
			mErrHandler.LogGLError ("FullGLImageDescriptorEntrypoint.CreateHandle");
			return (ulong)texHandle;
		}

		public void ReleaseHandle (ulong handle)
		{
			GL.Arb.MakeTextureHandleNonResident (handle);
			mErrHandler.LogGLError ("FullGLImageDescriptorEntrypoint.ReleaseHandle");
		}

		#endregion
	}
}

