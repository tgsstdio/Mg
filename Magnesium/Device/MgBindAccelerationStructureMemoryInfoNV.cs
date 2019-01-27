using System;

namespace Magnesium
{
	public class MgBindAccelerationStructureMemoryInfoNV
	{
		public IMgAcceleratedStructureNV AccelerationStructure { get; set; }
		public IMgDeviceMemory Memory { get; set; }
		public UInt64 MemoryOffset { get; set; }
		public UInt32[] DeviceIndices { get; set; }
	}
}
