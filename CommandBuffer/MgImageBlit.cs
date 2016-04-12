namespace Magnesium
{
    public class MgImageBlit
	{
		public MgImageSubresourceLayers SrcSubresource { get; set; }
		public MgOffset3D[] SrcOffsets { get; set; } // 2
		public MgImageSubresourceLayers DstSubresource { get; set; }
		public MgOffset3D[] DstOffsets { get; set; } // 2
	}
}

