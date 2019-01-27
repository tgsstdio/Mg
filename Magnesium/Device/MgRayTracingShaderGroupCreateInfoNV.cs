using System;

namespace Magnesium
{
	public class MgRayTracingShaderGroupCreateInfoNV
	{
		public MgRayTracingShaderGroupTypeNV Type { get; set; }
		public UInt32 GeneralShader { get; set; }
		public UInt32 ClosestHitShader { get; set; }
		public UInt32 AnyHitShader { get; set; }
		public UInt32 IntersectionShader { get; set; }
	}
}
