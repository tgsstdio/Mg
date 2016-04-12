using System;

namespace Magnesium
{
    public class MgImageCreateInfo
	{
		public MgImageCreateFlagBits Flags { get; set; }
		public MgImageType ImageType { get; set; }
		public MgFormat Format { get; set; }
		public MgExtent3D Extent { get; set; }
		public UInt32 MipLevels { get; set; }
		public UInt32 ArrayLayers { get; set; }
		public MgSampleCountFlagBits Samples { get; set; }
		public MgImageTiling Tiling { get; set; }
		public UInt32 Usage { get; set; }
		public MgSharingMode SharingMode { get; set; }
		public UInt32[] QueueFamilyIndices { get; set; }
		public MgImageLayout InitialLayout { get; set; }
	}
}

