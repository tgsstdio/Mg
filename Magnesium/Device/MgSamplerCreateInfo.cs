using System;

namespace Magnesium
{
    public class MgSamplerCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgFilter MagFilter { get; set; }
		public MgFilter MinFilter { get; set; }
		public MgSamplerMipmapMode MipmapMode { get; set; }
		public MgSamplerAddressMode AddressModeU { get; set; }
		public MgSamplerAddressMode AddressModeV { get; set; }
		public MgSamplerAddressMode AddressModeW { get; set; }
		public float MipLodBias { get; set; }
		public bool AnisotropyEnable { get; set; }
		public float MaxAnisotropy { get; set; }
		public bool CompareEnable { get; set; }
		public MgCompareOp CompareOp { get; set; }
		public float MinLod { get; set; }
		public float MaxLod { get; set; }
		public MgBorderColor BorderColor { get; set; }
		public bool UnnormalizedCoordinates { get; set; }
	}
}

