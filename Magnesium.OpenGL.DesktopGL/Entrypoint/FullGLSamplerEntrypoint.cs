using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLSamplerEntrypoint : IGLSamplerEntrypoint
	{
		private static All GetAddressMode(MgSamplerAddressMode mode)
		{
			switch (mode)
			{
			case MgSamplerAddressMode.CLAMP_TO_BORDER:
				return All.ClampToBorder;
			case MgSamplerAddressMode.CLAMP_TO_EDGE:
				return All.ClampToEdge;
			case MgSamplerAddressMode.MIRRORED_REPEAT:
				return All.MirroredRepeat;
				// EXT ARB_texture_mirror_clamp_to_edge
			case MgSamplerAddressMode.MIRROR_CLAMP_TO_EDGE:
				return All.MirrorClampToEdge;
			case MgSamplerAddressMode.REPEAT:
				return All.Repeat;
			default:
				throw new NotSupportedException();
			}
		}

		private static All GetMinFilterValue(MgFilter filter, MgSamplerMipmapMode mode)
		{
			switch (filter)
			{
			case MgFilter.LINEAR:
				return (mode == MgSamplerMipmapMode.LINEAR) ? All.LinearMipmapLinear : All.Linear;
			case MgFilter.NEAREST:
				return (mode == MgSamplerMipmapMode.LINEAR) ? All.NearestMipmapLinear : All.Nearest;
			default:
				throw new NotSupportedException();
			}
		}

		private static All GetMagFilterValue(MgFilter filter)
		{
			switch (filter)
			{
			case MgFilter.LINEAR:
				return All.Linear;
			case MgFilter.NEAREST:
				return All.Nearest;
			default:
				throw new NotSupportedException();
			}
		}

		private static All GetCompareOp (MgCompareOp compareOp)
		{
			switch (compareOp)
			{
			case MgCompareOp.ALWAYS:
				return All.Always;
			case MgCompareOp.EQUAL:
				return All.Equal;
			case MgCompareOp.LESS:
				return All.Less;
			case MgCompareOp.LESS_OR_EQUAL:
				return All.Lequal;
			case MgCompareOp.GREATER:
				return All.Greater;
			case MgCompareOp.GREATER_OR_EQUAL:
				return All.Gequal;
			case MgCompareOp.NOT_EQUAL:
				return All.Notequal;
			case MgCompareOp.NEVER:
				return All.Never;
			default:
				throw new NotSupportedException();
			}
		}

		readonly IGLErrorHandler mErrHandler;
		public FullGLSamplerEntrypoint (IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
		}

		#region IGLSamplerModule implementation

		public int CreateSampler ()
		{
            var result = GL.GenSampler ();
            mErrHandler.LogGLError("CreateSampler");
            return result;
        }

		public void DeleteSampler (int samplerId)
		{
			int id = samplerId;
			GL.DeleteSamplers (1, ref id);
            mErrHandler.LogGLError("DeleteSampler");
        }

		public void SetTextureWrapS (int samplerId, MgSamplerAddressMode addressModeU)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureWrapS, (int) GetAddressMode(addressModeU));
			mErrHandler.LogGLError ("SetTextureWrapS");
		}

		public void SetTextureWrapT (int samplerId, MgSamplerAddressMode addressModeV)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureWrapT, (int) GetAddressMode(addressModeV));
			mErrHandler.LogGLError ("SetTextureWrapT");
		}

		public void SetTextureWrapR (int samplerId, MgSamplerAddressMode addressModeW)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureWrapR, (int) GetAddressMode(addressModeW));
			mErrHandler.LogGLError ("SetTextureWrapR");
		}

		public void SetTextureMinLod (int samplerId, float minLod)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureMinLod, minLod);
			mErrHandler.LogGLError ("SetTextureMinLod");
		}

		public void SetTextureMaxLod (int samplerId, float maxLod)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureMaxLod, maxLod);
			mErrHandler.LogGLError ("SetTextureMaxLod");
		}

		public void SetTextureMinFilter (int samplerId, MgFilter minFilter, MgSamplerMipmapMode mipmapMode)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureMinFilter, (int) GetMinFilterValue(minFilter, mipmapMode));
			mErrHandler.LogGLError ("SetTextureMinFilter");
		}

		public void SetTextureMagFilter (int samplerId, MgFilter magFilter)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureMagFilter, (int) GetMagFilterValue(magFilter));
			mErrHandler.LogGLError ("SetTextureMagFilter");
		}

		public void SetTextureCompareFunc (int samplerId, MgCompareOp compareOp)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureCompareFunc, (int) GetCompareOp(compareOp) );
			mErrHandler.LogGLError ("SetTextureCompareFunc");
		}

		public void SetTextureBorderColorF (int samplerId, float[] color)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureBorderColor, color);
			mErrHandler.LogGLError ("SetTextureBorderColorF");
		}

		public void SetTextureBorderColorI (int samplerId, int[] color)
		{
			GL.SamplerParameter (samplerId, SamplerParameterName.TextureBorderColor, color);
			mErrHandler.LogGLError ("SetTextureBorderColorI");
		}
		#endregion
	}

}

