namespace Magnesium
{
    public class MgDisplayPlaneCapabilitiesKHR
	{
		public MgDisplayPlaneAlphaFlagBitsKHR SupportedAlpha { get; set; }
		public MgOffset2D MinSrcPosition { get; set; }
		public MgOffset2D MaxSrcPosition { get; set; }
		public MgExtent2D MinSrcExtent { get; set; }
		public MgExtent2D MaxSrcExtent { get; set; }
		public MgOffset2D MinDstPosition { get; set; }
		public MgOffset2D MaxDstPosition { get; set; }
		public MgExtent2D MinDstExtent { get; set; }
		public MgExtent2D MaxDstExtent { get; set; }
	}
}

