using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLCmdClearEntrypoint : IGLCmdClearEntrypoint
	{
        #region ICmdClearCapabilities implementation

        private IGLErrorHandler mErrHandler;
        public FullGLCmdClearEntrypoint(IGLErrorHandler handler)
        {
            mErrHandler = handler;
        }

		public GLClearValueState Initialize ()
		{
			var result = new GLClearValueState {
				ClearColor = new MgColor4f(0f, 0f, 0f, 0f),
				DepthValue = 1f,
				StencilValue = 0,
			};
			SetClearColor (result.ClearColor);
			SetClearDepthValue (result.DepthValue);
			SetClearStencilValue (result.StencilValue);
			return result; 
		}

		public void ClearBuffers (GLQueueClearBufferMask combinedMask)
		{
			ClearBufferMask bitmask = ((combinedMask & GLQueueClearBufferMask.Color) == GLQueueClearBufferMask.Color) ? ClearBufferMask.ColorBufferBit: 0;
			bitmask |= ((combinedMask & GLQueueClearBufferMask.Depth) == GLQueueClearBufferMask.Depth) ? ClearBufferMask.DepthBufferBit: 0;
			bitmask |= ((combinedMask & GLQueueClearBufferMask.Stencil) == GLQueueClearBufferMask.Stencil) ? ClearBufferMask.StencilBufferBit: 0;
			GL.Clear (bitmask);
		}

		public void SetClearStencilValue (uint stencil)
		{
			GL.ClearStencil ((int)stencil);
		}

		public void SetClearDepthValue (float value)
		{
			GL.ClearDepth (value);
		}

		public void SetClearColor (MgColor4f clearValue)
		{
			GL.ClearColor (clearValue.R, clearValue.G, clearValue.B, clearValue.A);
		}

        public void ClearFramebufferiv(uint index, MgVec4i clearValue)
        {
            GL.ClearBuffer(ClearBuffer.Color, (int)index, new[] { clearValue.X, clearValue.Y, clearValue.Z, clearValue.W });
            mErrHandler.LogGLError("ClearFramebufferiv");
        }

        public void ClearFramebufferuiv(uint index, MgVec4Ui clearValue)
        {
            GL.ClearBuffer(ClearBuffer.Color, (int)index, new[] { clearValue.X, clearValue.Y, clearValue.Z, clearValue.W });
            mErrHandler.LogGLError("ClearFramebufferuiv");
        }

        public void ClearFramebufferfv(uint index, MgColor4f clearValue)
        {
            GL.ClearBuffer(ClearBuffer.Color, (int)index, new[] { clearValue.R, clearValue.G, clearValue.B, clearValue.A });
            mErrHandler.LogGLError("ClearFramebufferfv");
        }

        public void ClearFramebufferDepthStencil(MgClearDepthStencilValue clearValue)
        {
            GL.ClearBuffer(ClearBufferCombined.DepthStencil, 0, clearValue.Depth, (int) clearValue.Stencil);
            mErrHandler.LogGLError("ClearFramebufferDepthStencil");
        }

        #endregion
    }
}

