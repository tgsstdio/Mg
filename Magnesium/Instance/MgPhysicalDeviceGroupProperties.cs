using System;

namespace Magnesium
{
	public class MgPhysicalDeviceGroupProperties
	{
		public IMgPhysicalDevice[] PhysicalDevices { get; set; }
		public bool SubsetAllocation { get; set; }
	}
}
