using System;

namespace Magnesium.Ktx
{
    public class KTXLoadInstructions
	{
		public KTXError Result;
		public UInt32 TextureDimensions;
		public MgImageViewType ViewType;
		public uint GlInternalFormat;
		public uint GlFormat;
		public bool IsCompressed;
		public bool GenerateMipmaps;
	};
}

