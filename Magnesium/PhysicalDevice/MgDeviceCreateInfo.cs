using System;

namespace Magnesium
{
    public class MgDeviceCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgDeviceQueueCreateInfo[] QueueCreateInfos { get; set; }
		public String[] EnabledLayerNames { get; set; }
		public String[] EnabledExtensionNames { get; set; }
		public MgPhysicalDeviceFeatures EnabledFeatures { get; set; }
	}
}

