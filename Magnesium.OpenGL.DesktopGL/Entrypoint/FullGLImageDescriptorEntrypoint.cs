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

		public long CreateHandle (int textureId, int samplerId)
		{
			long texHandle = GL.Arb.GetTextureSamplerHandle (textureId, samplerId);
			mErrHandler.LogGLError ("GetTextureSamplerHandle");
            GL.Arb.MakeTextureHandleResident(texHandle);
            mErrHandler.LogGLError("MakeTextureHandleResident");
            return texHandle;
		}

		public void ReleaseHandle (long handle)
		{
			GL.Arb.MakeTextureHandleNonResident (handle);
			mErrHandler.LogGLError (nameof(ReleaseHandle));
		}

		#endregion
	}
}

