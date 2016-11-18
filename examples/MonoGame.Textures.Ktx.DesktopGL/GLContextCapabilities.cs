using OpenTK.Graphics.OpenGL;

namespace MonoGame.Textures.Ktx.DesktopGL
{
	public class GLContextCapabilities : IGLContextCapabilities
	{
		public int MajorVersion;
		public int MinorVersion;
		public int ContextProfile;
		public bool SupportsSwizzle;
		public bool SupportsSRGB {get; private set;}
		public GLSizedFormats SizedFormats { get; private set; }
		public GLR16Formats R16Formats { get; private set; }

		public bool HasExtension(string ex)
		{
			// TODO : fix
			return true;
		}	

		public GLContextCapabilities ()
		{
			MajorVersion = 1;
			MinorVersion = 0;
			ContextProfile = 0;
			R16Formats = GLR16Formats.All;

			const int _CONTEXT_ES_PROFILE_BIT = 0x4;
			const int GL_CONTEXT_CORE_PROFILE_BIT = 0x00000001;
			const int GL_CONTEXT_COMPATIBILITY_PROFILE_BIT = 0x00000002;

			string versionDescription = GL.GetString(StringName.Version);

			if ( versionDescription.Contains("GL ES"))
			{
				ContextProfile = _CONTEXT_ES_PROFILE_BIT;
			}
			// MAJOR & MINOR only introduced in GL {,ES} 3.0
			GL.GetInteger(GetPName.MajorVersion, out MajorVersion);
			GL.GetInteger(GetPName.MinorVersion, out MinorVersion);
			if (GL.GetError() != ErrorCode.NoError) 
			{
				// < v3.0; resort to the old-fashioned way.
				if ((ContextProfile & _CONTEXT_ES_PROFILE_BIT) > 0)
				{
					string[] tokens = versionDescription.Split(new char[]{' ', '.'}, 4);
					MajorVersion = int.Parse(tokens[2]);
					MajorVersion = int.Parse(tokens[3]);
					//sscanf(versionDescription, "OpenGL ES %d.%d ", &majorVersion, &minorVersion);
				}
				else
				{
					string[] tokens = versionDescription.Split(new char[]{' ', '.'}, 3);
					MajorVersion = int.Parse(tokens[1]);
					MajorVersion = int.Parse(tokens[2]);					
					//sscanf(versionDescription, "OpenGL %d.%d ", &majorVersion, &minorVersion);
				}
			}
			if ((ContextProfile & _CONTEXT_ES_PROFILE_BIT) > 0)
			{
				if (MajorVersion < 3)
				{
					SupportsSwizzle = false;
					SizedFormats = GLSizedFormats.None;
					R16Formats = GLR16Formats.None;
					SupportsSRGB = false;
				} 
				else
				{
					SizedFormats = GLSizedFormats.Legacy;
				}
				if (HasExtension("GL_OES_required_internalformat"))
				{
					SizedFormats |= GLSizedFormats.All;
				}
				// There are no OES extensions for sRGB textures or R16 formats.
			} 
			else
			{
				// PROFILE_MASK was introduced in OpenGL 3.2.
				// Profiles: CONTEXT_CORE_PROFILE_BIT 0x1, CONTEXT_COMPATIBILITY_PROFILE_BIT 0x2.
				GL.GetInteger((GetPName) ((int) All.ContextProfileMask), out ContextProfile);
				if (GL.GetError() == ErrorCode.NoError) {
					// >= 3.2
					if (MajorVersion == 3 && MinorVersion < 3)
						SupportsSwizzle = false;
					if ((ContextProfile & GL_CONTEXT_CORE_PROFILE_BIT) > 0)
						SizedFormats &= ~GLSizedFormats.Legacy;
				}
				else
				{
					// < 3.2
					ContextProfile = GL_CONTEXT_COMPATIBILITY_PROFILE_BIT;
					SupportsSwizzle = false;
					// sRGB textures introduced in 2.0
					if (MajorVersion < 2 && HasExtension("GL_EXT_texture_sRGB")) 
					{
						SupportsSRGB = false;
					}
					// R{,G]16 introduced in 3.0; R{,G}16_SNORM introduced in 3.1.
					if (MajorVersion == 3)
					{
						if (MinorVersion == 0)
							R16Formats &= ~ GLR16Formats.SNorm;
					} else if (HasExtension("GL_ARB_texture_rg"))
					{
						R16Formats &= ~GLR16Formats.SNorm;
					}
					else
					{
						R16Formats = GLR16Formats.None;
					}
				}
			}
		}
	}
}

