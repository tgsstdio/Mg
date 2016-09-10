using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLCmdRasterizationEntrypoint : IGLCmdRasterizationEntrypoint
	{
		#region IRasterizerCapabilities implementation
		private IGLErrorHandler mErrHandler;
		public FullGLCmdRasterizationEntrypoint(IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
		}

		public void SetLineWidth (float width)
		{
			if (width > 0f)
			{
				GL.Enable (EnableCap.LineSmooth);
				GL.LineWidth (width);
				mErrHandler.LogGLError("SetLineWidth.Enable");
			} 
			else
			{
				GL.Disable (EnableCap.LineSmooth);
				mErrHandler.LogGLError("SetLineWidth.Disable");
			}
		}

		public void DisablePolygonOffset ()
		{
			GL.Disable(EnableCap.PolygonOffsetFill);
			mErrHandler.LogGLError("DisablePolygonOffset");
		}

		public void EnablePolygonOffset (float slopeScaleDepthBias, float depthBias)
		{
			GL.Enable(EnableCap.PolygonOffsetFill);
			GL.PolygonOffset(slopeScaleDepthBias, depthBias);
			mErrHandler.LogGLError("EnablePolygonOffset");
		}

		public void SetUsingCounterClockwiseWindings (bool flag)
		{
			if (flag)
			{
				GL.FrontFace (FrontFaceDirection.Ccw);
			} 
			else
			{
				GL.FrontFace (FrontFaceDirection.Cw);
			}

			mErrHandler.LogGLError("SetUsingCounterClockwiseWindings");
		}

		public void EnableScissorTest ()
		{
			GL.Enable(EnableCap.ScissorTest);
			mScissorTestEnabled = true;

			mErrHandler.LogGLError("EnableScissorTest");
		}

		public void DisableScissorTest ()
		{
			GL.Disable(EnableCap.ScissorTest);

			mScissorTestEnabled = false;

			mErrHandler.LogGLError("DisableScissorTest");
		}

		public void SetCullingMode (bool front, bool back)
		{
			if (front && back)
			{
				GL.CullFace (CullFaceMode.FrontAndBack);
			}
			else if (front)
			{
				GL.CullFace (CullFaceMode.Front);
			}
			else if (back)
			{
				GL.CullFace (CullFaceMode.Back);
			}
			else
			{
				// not sure about this
				DisableCulling ();
			}

			mErrHandler.LogGLError("SetCullingMode");
		}

		public void EnableCulling ()
		{
			GL.Enable(EnableCap.CullFace);
			mCullingEnabled = true;

			mErrHandler.LogGLError("EnableCulling");
		}

		public void DisableCulling ()
		{
			GL.Disable(EnableCap.CullFace);
			mCullingEnabled = false;

			mErrHandler.LogGLError("DisableCulling");
		}

		public GLQueueRendererRasterizerState Initialize ()
		{
			var initialValue = new GLQueueRendererRasterizerState {
				Flags = 
					GLGraphicsPipelineFlagBits.ScissorTestEnabled 
					| GLGraphicsPipelineFlagBits.CullBackFaces 
					| GLGraphicsPipelineFlagBits.UseCounterClockwiseWindings,
					// ! QueueDrawItemBitFlags.CullingEnabled,
				DepthBias = new GLCmdDepthBiasParameter
				{
					DepthBiasClamp = 0,
					DepthBiasConstantFactor = 0,
					DepthBiasSlopeFactor = 0,
				},
				LineWidth = 1f,
			};

			EnableScissorTest ();
			DisableCulling ();

			SetCullingMode (
				(initialValue.Flags & GLGraphicsPipelineFlagBits.CullFrontFaces)
				== GLGraphicsPipelineFlagBits.CullFrontFaces,
				(initialValue.Flags & GLGraphicsPipelineFlagBits.CullBackFaces)
				== GLGraphicsPipelineFlagBits.CullBackFaces);
			SetUsingCounterClockwiseWindings (
				(initialValue.Flags & GLGraphicsPipelineFlagBits.UseCounterClockwiseWindings)
					== GLGraphicsPipelineFlagBits.UseCounterClockwiseWindings
			);

			DisablePolygonOffset ();

			SetLineWidth (initialValue.LineWidth);

			return initialValue;
		}

		private bool mScissorTestEnabled;
		public bool ScissorTestEnabled {
			get {
				return mScissorTestEnabled;
			}
		}

		private bool mCullingEnabled;
		public bool CullingEnabled {
			get {
				return mCullingEnabled;
			}
		}

		#endregion


	}
}

