using System;

namespace Magnesium
{
	[Flags]
	public enum MgSubgroupFeatureFlagBits : UInt32
	{
		/// <summary> 
		/// Basic subgroup operations
		/// </summary> 
		BASIC_BIT = 0x1,
		/// <summary> 
		/// Vote subgroup operations
		/// </summary> 
		VOTE_BIT = 0x2,
		/// <summary> 
		/// Arithmetic subgroup operations
		/// </summary> 
		ARITHMETIC_BIT = 0x4,
		/// <summary> 
		/// Ballot subgroup operations
		/// </summary> 
		BALLOT_BIT = 0x8,
		/// <summary> 
		/// Shuffle subgroup operations
		/// </summary> 
		SHUFFLE_BIT = 0x10,
		/// <summary> 
		/// Shuffle relative subgroup operations
		/// </summary> 
		SHUFFLE_RELATIVE_BIT = 0x20,
		/// <summary> 
		/// Clustered subgroup operations
		/// </summary> 
		CLUSTERED_BIT = 0x40,
		/// <summary> 
		/// Quad subgroup operations
		/// </summary> 
		QUAD_BIT = 0x80,
		PARTITIONED_BIT_NV = 0x100,
	}
}
