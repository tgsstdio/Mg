using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLCmdDepthEntrypoint : IGLCmdDepthEntrypoint
	{
        #region IDepthCapabilities implementation

        private IGLErrorHandler mErrHandler;
        public FullGLCmdDepthEntrypoint(IGLErrorHandler handler)
        {
            mErrHandler = handler;
        }

		public GLGraphicsPipelineDepthState GetDefaultEnums ()
		{
			return new GLGraphicsPipelineDepthState{ DepthBufferFunction = MgCompareOp.LESS };
		}

		public GLGraphicsPipelineDepthState Initialize ()
		{
			EnableDepthBuffer ();
			SetDepthBufferFunc (MgCompareOp.LESS);
			SetDepthMask(true);
			return GetDefaultEnums ();
		}

		private bool mIsDepthBufferEnabled;
		public bool IsDepthBufferEnabled {
			get {
				return mIsDepthBufferEnabled;
			}
		}

		public void EnableDepthBuffer ()
		{
			GL.Enable(EnableCap.DepthTest);
			mIsDepthBufferEnabled = true;
            mErrHandler.LogGLError("EnableDepthBuffer");
		}

		public void DisableDepthBuffer ()
		{
			GL.Disable(EnableCap.DepthTest);
			mIsDepthBufferEnabled = false;
            mErrHandler.LogGLError("DisableDepthBuffer");
        }

		private static DepthFunction GetDepthFunction(MgCompareOp compare)
		{
			switch (compare)
			{
			default:
				throw new NotSupportedException ();
			case MgCompareOp.ALWAYS:
				return DepthFunction.Always;
			case MgCompareOp.EQUAL:
				return DepthFunction.Equal;
			case MgCompareOp.GREATER:
				return DepthFunction.Greater;
			case MgCompareOp.GREATER_OR_EQUAL:
				return DepthFunction.Gequal;
			case MgCompareOp.LESS:
				return DepthFunction.Less;
			case MgCompareOp.LESS_OR_EQUAL:
				return DepthFunction.Lequal;
			case MgCompareOp.NEVER:
				return DepthFunction.Never;
			case MgCompareOp.NOT_EQUAL:
				return DepthFunction.Notequal;
			}
		}

		public void SetDepthBufferFunc(MgCompareOp func)
		{
			GL.DepthFunc (GetDepthFunction (func));
            mErrHandler.LogGLError("SetDepthBufferFunc");
        }

		public void SetDepthMask (bool isMaskOn)
		{
			// for writing to depth buffer
			GL.DepthMask(isMaskOn);
            mErrHandler.LogGLError("SetDepthMask");
        }

		public void SetClipControl(bool usingLowerLeftCorner, bool zeroToOneRange)
		{
			
		}

        public void SetDepthBounds(float min, float max)
        {
            GL.Ext.DepthBounds(min, max);
            mErrHandler.LogGLError("SetDepthBounds");
        }
        #endregion
    }
}

