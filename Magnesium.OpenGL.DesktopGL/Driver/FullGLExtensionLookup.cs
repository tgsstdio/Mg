using System.Collections.Specialized;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLExtensionLookup : IGLExtensionLookup
	{
		public StringCollection Extensions {get; private set;}		
		public FullGLExtensionLookup ()
		{
			Extensions = new StringCollection();
		}

		public void Initialize()
		{
			Extensions.Clear ();

			AfterVersion3_0 ();

			ProirToVersion3_0 ();
		}

		public bool HasExtension (string extension)
		{
			return Extensions.Contains(extension);
		}

//		void PreviousImpl()
//		{
//			var extstring = GL.GetString(StringName.Extensions);
//			GraphicsExtensions.CheckGLError();
//			if (!string.IsNullOrEmpty(extstring))
//			{
//				Extensions.AddRange(extstring.Split(' '));
//				System.Diagnostics.Debug.WriteLine("Supported extensions:");
//
//				foreach (string extension in Extensions)
//				{
//					System.Diagnostics.Debug.WriteLine(extension);
//				}
//			}
//		}

		void AfterVersion3_0 ()
		{
			int count = GL.GetInteger (GetPName.NumExtensions);
			for (int i = 0; i < count; i++)
			{
				string extension = GL.GetString (StringNameIndexed.Extensions, i);
				Extensions.Add (extension);
			}
		}

		private void ProirToVersion3_0 ()
		{
			string extension_string = GL.GetString (StringName.Extensions);
			foreach (string extension in extension_string.Split (' '))
			{
				Extensions.Add (extension);
			}
		}
	}
}

