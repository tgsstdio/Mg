using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLCmdShaderProgramCache : IGLCmdShaderProgramCache
	{
		#region IShaderProgramCache implementation
		public FullGLCmdShaderProgramCache ()
		{
			mProgramID = 0;
			mVBO = 0;
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
				}
			}
		}


		public byte DescriptorSetIndex {
			get;
			set;
		}

		public GLCmdDescriptorSetParameter DescriptorSet {
			get;
			set;
		}

		#endregion

		public void BindDescriptorSet ()
		{
			
		}
	}
}

