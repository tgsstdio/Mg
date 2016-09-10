using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLDevicePlatform : IGLDevicePlatform
	{
		private DrawBuffersEnum[] _drawBuffers;

		IGLErrorHandler mErrHandler;

		public FullGLDevicePlatform (IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
		}

		public void Initialize ()
		{
			int maxTextures = 16;
			GL.GetInteger(GetPName.MaxTextureImageUnits, out maxTextures);
			mErrHandler.CheckGLError();
			MaxTextureSlots = maxTextures;

			int maxVertexAttribs = 0;
			GL.GetInteger(GetPName.MaxVertexAttribs, out maxVertexAttribs);
			mErrHandler.CheckGLError();
			MaxVertexAttributes = maxVertexAttribs;

			int texSize = 0;
			GL.GetInteger(GetPName.MaxTextureSize, out texSize);
			mErrHandler.CheckGLError();		
			MaxTextureSize = texSize;

			// Initialize draw buffer attachment array
			int maxDrawBuffers;
			GL.GetInteger(GetPName.MaxDrawBuffers, out maxDrawBuffers);
			_drawBuffers = new DrawBuffersEnum[maxDrawBuffers];
			for (int i = 0; i < maxDrawBuffers; i++)
				_drawBuffers[i] = (DrawBuffersEnum)(FramebufferAttachment.ColorAttachment0Ext + i);
		}

		public void AfterApplyRenderTargets (int renderCount)
		{
			GL.DrawBuffers(renderCount, this._drawBuffers);
		}

		#region IGLDevicePlatform implementation

		public int MaxVertexAttributes {
			get;
			private set;
		}

		public int MaxTextureSize {
			get;
			private set;
		}

		public int MaxTextureSlots {
			get;
			private set;
		}

		#endregion
	}
}

