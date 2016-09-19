using System;
using OpenTK.Graphics.ES30;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class ES30GLErrorHandler : IGLErrorHandler
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
			catch (MgOpenGLException ex)
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

