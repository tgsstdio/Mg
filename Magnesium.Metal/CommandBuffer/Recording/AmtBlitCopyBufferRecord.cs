using System;
using Metal;

namespace Magnesium.Metal
{
	public struct AmtBlitCopyBufferRegionRecord
	{
		public nuint DestinationOffset { get; set;}
		public nuint SourceOffset { get; set;}
		public nuint Size { get; set; }
	}

	public class AmtBlitCopyBufferRecord
	{
		public IMTLBuffer Src { get; set; }
		public IMTLBuffer Dst { get; set; }
		public AmtBlitCopyBufferRegionRecord[] Regions { get; set;}
	}
}