using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLCmdScissorsEntrypoint : IGLCmdScissorsEntrypoint
	{
		private readonly IGLErrorHandler mErrHandler;
		public FullGLCmdScissorsEntrypoint(IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
		}

		#region IScissorsCapabilities implementation

		public void ApplyScissors (GLCmdScissorParameter scissors)
		{
			GL.ScissorArray (scissors.Parameters.First, scissors.Parameters.Count, scissors.Parameters.Values);
			mErrHandler.LogGLError("ApplyScissors");
		}


		public void ApplyViewports (GLCmdViewportParameter viewports)
		{			
			GL.ViewportArray (viewports.Viewport.First, viewports.Viewport.Count, viewports.Viewport.Values);

			mErrHandler.LogGLError("ApplyViewports.ViewportArray");

			GL.DepthRangeArray (viewports.DepthRange.First, viewports.DepthRange.Count, viewports.DepthRange.Values);

			mErrHandler.LogGLError("ApplyViewports.DepthRangeArray");
		}

		#endregion
	}
}

