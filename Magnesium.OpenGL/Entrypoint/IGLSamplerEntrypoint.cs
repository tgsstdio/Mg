namespace Magnesium.OpenGL
{
	public interface IGLSamplerEntrypoint
	{
		int CreateSampler();

		void DeleteSampler (int samplerId);

		void SetTextureWrapS (int samplerId, MgSamplerAddressMode addressModeU);

		void SetTextureWrapT (int samplerId, MgSamplerAddressMode addressModeV);

		void SetTextureWrapR (int samplerId, MgSamplerAddressMode addressModeW);

		void SetTextureMinLod (int samplerId, float minLod);

		void SetTextureMaxLod (int samplerId, float maxLod);

		void SetTextureMinFilter (int samplerId, MgFilter minFilter, MgSamplerMipmapMode mipmapMode);

		void SetTextureMagFilter (int samplerId, MgFilter magFilter);

		void SetTextureCompareFunc (int samplerId, MgCompareOp compareOp);

		void SetTextureBorderColorF (int samplerId, float[] color);
		void SetTextureBorderColorI (int samplerId, int[] color);
	}
}

