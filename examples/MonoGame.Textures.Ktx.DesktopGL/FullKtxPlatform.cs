using System;
using OpenTK.Graphics.OpenGL;

namespace MonoGame.Textures.Ktx.DesktopGL
{
	public class FullKtxPlatform : IKtxPlatform
	{
		#region IKtxPlatform implementation

		public uint GetTexTargetTexture2DArrayEnumValue ()
		{
			return (uint) TextureTarget.Texture2DArray;
		}

		public uint GetTexTargetTexture1DArrayEnumValue ()
		{
			return (uint) TextureTarget.Texture1DArray;
		}

		public uint GetTexTargetTexture1DEnumValue ()
		{
			return (uint)TextureTarget.Texture1D;
		}

		public uint GetTexTargetTexture2DEnumValue ()
		{
			return (uint)TextureTarget.Texture2D;
		}

		public uint GetTexTargetTexture3DEnumValue ()
		{
			return (uint)TextureTarget.Texture3D;
		}

		public uint GetTexTargetCubeMapEnumValue ()
		{
			return (uint)TextureTarget.TextureCubeMap;
		}

		public bool IsErrorFound (int error)
		{
			return error != (int) ErrorCode.NoError;
		}

		public int GetError ()
		{
			return (int)GL.GetError ();
		}

		public int GetNoErrorEnumValue ()
		{
			return (int)ErrorCode.NoError;
		}

		public uint GetTextureCubeMapFirstFace ()
		{
			return (uint)TextureTarget.TextureCubeMapPositiveX;
		}

		public uint GetTextureCubeMapEnumValue ()
		{
			return (uint)TextureTarget.TextureCubeMap;
		}

		public AtlasTextureTarget ConvertTargetType (uint glTarget)
		{
			TextureTarget target = (TextureTarget) glTarget;
			switch (target)
			{
			case TextureTarget.Texture1D:
				return AtlasTextureTarget.Texture1D;
			case TextureTarget.Texture2D:
				return AtlasTextureTarget.Texture2D;
			case TextureTarget.TextureCubeMapPositiveX:
				return AtlasTextureTarget.TextureCubeMap;
			default:
				throw new NotSupportedException ();
			}

		}

		public int GetUnpackAlignment ()
		{
			/* KTX files require an unpack alignment of 4 */
			int previousUnpackAlignment;
			GL.GetInteger (GetPName.UnpackAlignment, out previousUnpackAlignment);
			return previousUnpackAlignment;
		}

		public void SetUnpackedAlignment (int alignment)
		{
			GL.PixelStore (PixelStoreParameter.UnpackAlignment, alignment);
		}

		public bool CheckInternalFormat (KTXHeader header)
		{
			return (header.GlBaseInternalFormat == (UInt32)PixelInternalFormat.Alpha || header.GlBaseInternalFormat == (UInt32)PixelInternalFormat.Luminance || header.GlBaseInternalFormat == (UInt32)PixelInternalFormat.LuminanceAlpha || header.GlBaseInternalFormat == (UInt32)PixelInternalFormat.Intensity);
		}

		#endregion
	}
}

