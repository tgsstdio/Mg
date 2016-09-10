using System.Collections.Specialized;
using OpenTK.Graphics.ES20;

namespace Magnesium.OpenGL.AndroidGL
{
	public class Es20GLSpecificExtensionLookup : IGLExtensionLookup
	{
		public StringCollection Extensions {get; private set;}

		public Es20GLSpecificExtensionLookup ()
		{
			Extensions = new StringCollection();			
		}

		public void Initialize()
		{
			Extensions.Clear ();

			ProirToVersion3_0 ();
		}

		private void ProirToVersion3_0 ()
		{
			string extension_string = GL.GetString (All.Extensions);
			foreach (string extension in extension_string.Split (' '))
			{
				Extensions.Add (extension);
			}
		}

		public bool HasExtension (string extension)
		{
			return Extensions.Contains(extension);
		}
	}
}

