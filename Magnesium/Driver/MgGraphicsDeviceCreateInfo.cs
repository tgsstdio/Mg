using System;

namespace Magnesium
{
	public class MgGraphicsDeviceCreateInfo
	{
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set;}
        public MgColorFormatOption Color { get; set; }
		public MgFormat OverrideColor { get; set; }
        public MgDepthFormatOption DepthStencil { get; set; }
		public MgFormat OverrideDepthStencil { get; set;}
        public MgSampleCountFlagBits Samples { get; set; }
	}
}

