using System;

namespace Magnesium
{
	public class MgAccelerationStructureInfoNV
	{
		public MgAccelerationStructureTypeNV Type { get; set; }
		public MgBuildAccelerationStructureFlagBitsNV Flags { get; set; }
		public UInt32 InstanceCount { get; set; }
		public MgGeometryNV[] Geometries { get; set; }
	}
}
