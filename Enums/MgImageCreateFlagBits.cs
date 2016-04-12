using System;

namespace Magnesium
{
    [Flags] 
	public enum MgImageCreateFlagBits : byte
	{
		// Image should support sparse backing
		SPARSE_BINDING_BIT = 1 << 0,
		// Image should support sparse backing with partial residency
		SPARSE_RESIDENCY_BIT = 1 << 1,
		// Image should support constent data access to physical memory blocks mapped into multiple locations of sparse images
		SPARSE_ALIASED_BIT = 1 << 2,
		// Allows image views to have different format than the base image
		MUTABLE_FORMAT_BIT = 1 << 3,
		// Allows creating image views with cube type from the created image
		CUBE_COMPATIBLE_BIT = 1 << 4,
	}
}

