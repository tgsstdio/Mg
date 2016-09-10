using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLErrorHandler : IGLErrorHandler
	{
		#region IGLErrorHandler implementation

		public void CheckGLError()
		{
			var error = GL.GetError();
			if (error != ErrorCode.NoError)
				throw new MgOpenGLException("GL.GetError() returned " + error);
		}

		public void LogGLError (string location)
		{
			try
			{
				CheckGLError();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("MonoGameGLException at " + location + " - " + ex.Message);
			}
		}

		public void Trace(string message)
		{

		}

		#endregion
	}
}

