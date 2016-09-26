using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtSampler : IMgSampler
	{
		IMTLSamplerState mSampler;

		public AmtSampler(IMTLDevice mDevice, MgSamplerCreateInfo pCreateInfo)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			var descriptor = new MTLSamplerDescriptor
			{
				SAddressMode = TranslateAddressMode(pCreateInfo.AddressModeU),
				TAddressMode = TranslateAddressMode(pCreateInfo.AddressModeV),
				RAddressMode = TranslateAddressMode(pCreateInfo.AddressModeW),
				MinFilter = TranslateMinFilter(pCreateInfo.MinFilter),
				MagFilter = TranslateMagFilter(pCreateInfo.MagFilter),
				MipFilter = TranslateMipFilter(pCreateInfo.MipmapMode),
				LodMinClamp = pCreateInfo.MinLod,
				LodMaxClamp = pCreateInfo.MaxLod,
				MaxAnisotropy = (nuint) pCreateInfo.MaxAnisotropy, 
				CompareFunction = TranslateCompareFunction(pCreateInfo.CompareOp),
				NormalizedCoordinates = !pCreateInfo.UnnormalizedCoordinates,

			};
			
			mSampler = mDevice.CreateSamplerState(descriptor);

		}

		MTLCompareFunction TranslateCompareFunction(MgCompareOp compareOp)
		{
			switch (compareOp)
			{
				default:
					throw new NotSupportedException();
				case MgCompareOp.ALWAYS:
					return MTLCompareFunction.Always;
				case MgCompareOp.EQUAL:
					return MTLCompareFunction.Equal;
				case MgCompareOp.GREATER:
					return MTLCompareFunction.Greater;
				case MgCompareOp.GREATER_OR_EQUAL:
					return MTLCompareFunction.GreaterEqual;
				case MgCompareOp.LESS:
					return MTLCompareFunction.Less;
				case MgCompareOp.LESS_OR_EQUAL:
					return MTLCompareFunction.LessEqual;
				case MgCompareOp.NEVER:
					return MTLCompareFunction.Never;
				case MgCompareOp.NOT_EQUAL:
					return MTLCompareFunction.NotEqual;

			}
		}

		MTLSamplerMipFilter TranslateMipFilter(MgSamplerMipmapMode mode)
		{
			switch (mode)
			{
				default:
					throw new NotSupportedException();
				case MgSamplerMipmapMode.LINEAR:
					return MTLSamplerMipFilter.Linear;
				case MgSamplerMipmapMode.NEAREST:
					return MTLSamplerMipFilter.Nearest;
			}

		}

		MTLSamplerMinMagFilter TranslateMagFilter(MgFilter filter)
		{
			switch (filter)
			{
				default:
					throw new NotSupportedException();
				case MgFilter.LINEAR:
					return MTLSamplerMinMagFilter.Linear;
				case MgFilter.NEAREST:
					return MTLSamplerMinMagFilter.Nearest;
			}
		}

		MTLSamplerMinMagFilter TranslateMinFilter(MgFilter filter)
		{
			switch (filter)
			{
				default:
					throw new NotSupportedException();
				case MgFilter.LINEAR:
					return MTLSamplerMinMagFilter.Linear;
				case MgFilter.NEAREST:
					return MTLSamplerMinMagFilter.Nearest;
			}
		}

		MTLSamplerAddressMode TranslateAddressMode(MgSamplerAddressMode addressMode)
		{
			switch (addressMode)
			{
				default:
					throw new NotSupportedException();
				case MgSamplerAddressMode.CLAMP_TO_EDGE:
				return MTLSamplerAddressMode.ClampToEdge;
				case MgSamplerAddressMode.MIRRORED_REPEAT:
					return MTLSamplerAddressMode.MirrorRepeat;
				case MgSamplerAddressMode.MIRROR_CLAMP_TO_EDGE:
					return MTLSamplerAddressMode.MirrorClampToEdge;
				case MgSamplerAddressMode.REPEAT:
					return MTLSamplerAddressMode.Repeat;
			}

		}

		public void DestroySampler(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
	}
}