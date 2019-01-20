using System;

namespace Magnesium
{
	public class MgPhysicalDeviceImageFormatInfo2
	{
		public MgFormat Format { get; set; }
		public MgImageType Type { get; set; }
		public MgImageTiling Tiling { get; set; }
		public MgImageUsageFlagBits Usage { get; set; }
		public MgImageCreateFlagBits Flags { get; set; }
	}
}
