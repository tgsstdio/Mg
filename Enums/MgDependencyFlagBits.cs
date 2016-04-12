using System;

namespace Magnesium
{
    [Flags] 
	public enum MgDependencyFlagBits : byte
	{
		// Dependency is per pixel region 
		VK_DEPENDENCY_BY_REGION_BIT = 1 << 0,
	}
}

