using System;

namespace Magnesium
{
	public class MgGraphicsDeviceCreateInfo
	{
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set;}
		public MgFormat Color { get; set; }
		public MgFormat DepthStencil { get; set;}
		public MgSampleCountFlagBits Samples { get; set; }
	}
}

