using System;

namespace Magnesium
{
    public class MgPhysicalDeviceProperties
	{
		public UInt32 ApiVersion { get; set; }
		public UInt32 DriverVersion { get; set; }
		public UInt32 VendorID { get; set; }
		public UInt32 DeviceID { get; set; }
		public MgPhysicalDeviceType DeviceType { get; set; }
		public String DeviceName { get; set; }
		public Guid PipelineCacheUUID { get; set; }
		public MgPhysicalDeviceLimits Limits { get; set; }
		public MgPhysicalDeviceSparseProperties SparseProperties { get; set; }
	}
}

