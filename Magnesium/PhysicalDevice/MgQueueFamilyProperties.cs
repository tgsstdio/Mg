using System;

namespace Magnesium
{
    public class MgQueueFamilyProperties
	{
		public MgQueueFlagBits QueueFlags { get; set; }
		public UInt32 QueueCount { get; set; }
		public UInt32 TimestampValidBits { get; set; }
		public MgExtent3D MinImageTransferGranularity { get; set; }
	}
}

