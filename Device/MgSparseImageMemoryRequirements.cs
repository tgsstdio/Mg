using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgSparseImageMemoryRequirements
	{
		public MgSparseImageFormatProperties FormatProperties { get; set; }
		public UInt32 ImageMipTailFirstLod { get; set; }
		public UInt64 ImageMipTailSize { get; set; }
		public UInt64 ImageMipTailOffset { get; set; }
		public UInt64 ImageMipTailStride { get; set; }
	}
}

