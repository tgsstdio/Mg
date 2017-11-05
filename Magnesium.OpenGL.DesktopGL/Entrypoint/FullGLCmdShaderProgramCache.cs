using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLCmdShaderProgramCache : IGLCmdShaderProgramCache
	{
        #region IShaderProgramCache implementation
        private IGLErrorHandler mErrHandler;
		public FullGLCmdShaderProgramCache (IGLErrorHandler errHandler)
		{
			mProgramID = 0;
			mVBO = 0;
            mErrHandler = errHandler;
        }

		private int mProgramID;
		public int ProgramID {
			get {
				return mProgramID;
			}
			set {
				if (mProgramID != value)
				{
					mProgramID = value;
					GL.UseProgram (mProgramID);
                    mErrHandler.LogGLError("UseProgram");
                }
			}
		}

		private int mVBO;
		public int VBO {
			get {
				return mVBO;
			}
			set {
				if (mVBO != value)
				{
					mVBO = value;
					GL.BindVertexArray (mVBO);
                    mErrHandler.LogGLError("BindVertexArray");
                }
			}
		}


		public byte DescriptorSetIndex {
			get;
			set;
		}

		#endregion

		public void BindDescriptorSet ()
		{
			
		}
	}
}

