using System;
using Magnesium;

namespace Magnesium.Mtx
{
	public class MtxMipmapData
	{
		public MgImageViewType ViewType { get; set; }
		public int SizeRounded { get; set; }
		public int FaceIndex { get; internal set; }
		public byte[] Data { get; set; }
		public UInt32 Size { get; set; }
		public UInt32 PixelWidth { get; internal set; }
		public UInt32 PixelHeight { get; internal set; }
		public UInt32 PixelDepth { get; internal set; }
		public int Level { get; internal set; }
	}
}

