using System;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
	public class GLSampler : IMgSampler
	{
		public int SamplerId { get; private set; }

		IGLSamplerEntrypoint mEntrypoint;

		public GLSampler (int samplerId, MgSamplerCreateInfo pCreateInfo, IGLSamplerEntrypoint entrypoint)
		{
			SamplerId = samplerId;
			mEntrypoint = entrypoint;
			Populate (pCreateInfo);
		}

		private void Populate (MgSamplerCreateInfo pCreateInfo)
		{
			Debug.Assert(mEntrypoint != null);
			Debug.Assert(pCreateInfo != null);
			// ARB_SAMPLER_OBJECTS
			mEntrypoint.SetTextureWrapS(SamplerId, pCreateInfo.AddressModeU);
			//GL.SamplerParameter (SamplerId, SamplerParameterName.TextureWrapS, (int) GetAddressMode(pCreateInfo.AddressModeU));

			mEntrypoint.SetTextureWrapT(SamplerId, pCreateInfo.AddressModeV);
			//GL.SamplerParameter (SamplerId, SamplerParameterName.TextureWrapT, (int) GetAddressMode(pCreateInfo.AddressModeV));

			mEntrypoint.SetTextureWrapR(SamplerId, pCreateInfo.AddressModeW);
			//GL.SamplerParameter (SamplerId, SamplerParameterName.TextureWrapR, (int) GetAddressMode(pCreateInfo.AddressModeW));

			mEntrypoint.SetTextureMinLod(SamplerId, pCreateInfo.MinLod);
			//GL.SamplerParameter (SamplerId, SamplerParameterName.TextureMinLod, pCreateInfo.MinLod);

			mEntrypoint.SetTextureMaxLod(SamplerId, pCreateInfo.MaxLod);
			//GL.SamplerParameter (SamplerId, SamplerParameterName.TextureMaxLod, pCreateInfo.MaxLod);

			mEntrypoint.SetTextureMinFilter(SamplerId, pCreateInfo.MinFilter, pCreateInfo.MipmapMode);
			//GL.SamplerParameter (SamplerId, SamplerParameterName.TextureMinFilter, (int) GetMinFilterValue(pCreateInfo.MinFilter, pCreateInfo.MipmapMode));

			mEntrypoint.SetTextureMagFilter(SamplerId, pCreateInfo.MagFilter);
			//GL.SamplerParameter (SamplerId, SamplerParameterName.TextureMagFilter, (int) GetMagFilterValue(pCreateInfo.MagFilter));

			mEntrypoint.SetTextureCompareFunc(SamplerId, pCreateInfo.CompareOp);
			//GL.SamplerParameter (SamplerId, SamplerParameterName.TextureCompareFunc, (int) GetCompareOp(pCreateInfo.CompareOp) );

			// EXT_texture_filter_anisotropic
			//GL.SamplerParameter (samplerId, SamplerParameterName.TextureMaxAnisotropyExt, pCreateInfo.MaxAnisotropy);

			switch (pCreateInfo.BorderColor)
			{
			case MgBorderColor.FLOAT_OPAQUE_BLACK:
				var FLOAT_OPAQUE_BLACK = new float[] {
					0f,
					0f,
					0f,
					1f
				};
				mEntrypoint.SetTextureBorderColorF (SamplerId, FLOAT_OPAQUE_BLACK);
				break;
			case MgBorderColor.FLOAT_OPAQUE_WHITE:
				var FLOAT_OPAQUE_WHITE = new float[] {
					1f,
					1f,
					1f,
					1f
				};
				mEntrypoint.SetTextureBorderColorF (SamplerId, FLOAT_OPAQUE_WHITE);
				break;
			case MgBorderColor.FLOAT_TRANSPARENT_BLACK:
				var FLOAT_TRANSPARENT_BLACK = new float[] {
					0f,
					0f,
					0f,
					0f
				};
				mEntrypoint.SetTextureBorderColorF (SamplerId, FLOAT_TRANSPARENT_BLACK);
				break;
			case MgBorderColor.INT_OPAQUE_BLACK:
				var INT_OPAQUE_BLACK = new int[] {
					0,
					0,
					0,
					255
				};
				mEntrypoint.SetTextureBorderColorI (SamplerId, INT_OPAQUE_BLACK);
				break;
			case MgBorderColor.INT_OPAQUE_WHITE:
				var INT_OPAQUE_WHITE = new int[] {
					255,
					255,
					255,
					255
				};
				mEntrypoint.SetTextureBorderColorI (SamplerId, INT_OPAQUE_WHITE);
				break;
			case MgBorderColor.INT_TRANSPARENT_BLACK:
				var INT_TRANSPARENT_BLACK = new int[] {
					0,
					0,
					0,
					0
				};
				mEntrypoint.SetTextureBorderColorI (SamplerId, INT_TRANSPARENT_BLACK);
				break;
			}

		}

		#region IMgSampler implementation
		private bool mIsDisposed = false;
		public void DestroySampler (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mEntrypoint.DeleteSampler (SamplerId);

			mIsDisposed = true;
		}

		#endregion
	}
}

