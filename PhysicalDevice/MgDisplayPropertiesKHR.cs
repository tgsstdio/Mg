using System;

namespace Magnesium
{
    public class MgDisplayPropertiesKHR
	{
		public MgDisplayKHR Display { get; set; }
		public String DisplayName { get; set; }
		public MgExtent2D PhysicalDimensions { get; set; }
		public MgExtent2D PhysicalResolution { get; set; }
		public MgSurfaceTransformFlagBitsKHR SupportedTransforms { get; set; }
		public bool PlaneReorderPossible { get; set; }
		public bool PersistentContent { get; set; }
	}
}

