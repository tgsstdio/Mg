namespace Magnesium
{
    public class MgPhysicalDeviceSparseImageFormatInfo2
	{
		public MgFormat Format { get; set; }
		public MgImageType Type { get; set; }
		public MgSampleCountFlagBits Samples { get; set; }
		public MgImageUsageFlagBits Usage { get; set; }
		public MgImageTiling Tiling { get; set; }
	}
}
