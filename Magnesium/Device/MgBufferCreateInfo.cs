using System;

namespace Magnesium
{
    public class MgBufferCreateInfo
	{
		public MgBufferCreateFlagBits Flags { get; set; }
		public UInt64 Size { get; set; }
		public MgBufferUsageFlagBits Usage { get; set; }
		public MgSharingMode SharingMode { get; set; }
		public UInt32 QueueFamilyIndexCount { get; set; }
		public UInt32[] QueueFamilyIndices { get; set; }
	}
}

