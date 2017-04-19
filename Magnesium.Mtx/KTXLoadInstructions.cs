using System;
using Magnesium;

namespace Magnesium.Mtx
{
    public class KTXLoadInstructions
	{
		public KTXError Result;
		public UInt32 TextureDimensions;
		public MgImageType ImageType;
		public MgImageViewType ViewType;
		public MgFormat Format;
		public uint GlInternalFormat;
		public uint GlFormat;
		public bool IsCompressed;
		public bool GenerateMipmaps;
	};
}

