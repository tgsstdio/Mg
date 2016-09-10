namespace Magnesium
{
	public class MgImageSource
	{
		public MgFormat Format { get; set; }
		public uint Size { get; set; } 
		public uint Width { get; set; }
		public uint Height { get; set; }
		public MgImageMipmap[] Mipmaps { get; set; }
	}
}

