using System;

namespace Magnesium
{
    public class MgDeviceQueueCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 QueueFamilyIndex { get; set; }
		public UInt32 QueueCount { get; set; }
		public float[] QueuePriorities { get; set; }
	}
}

