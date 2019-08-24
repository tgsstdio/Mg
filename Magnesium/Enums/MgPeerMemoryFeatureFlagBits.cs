using System;

namespace Magnesium
{
	[Flags]
	public enum MgPeerMemoryFeatureFlagBits : UInt32
	{
		/// <summary> 
		/// Can read with vkCmdCopy commands
		/// </summary> 
		COPY_SRC_BIT = 0x1,
		/// <summary> 
		/// Can write with vkCmdCopy commands
		/// </summary> 
		COPY_DST_BIT = 0x2,
		/// <summary> 
		/// Can read with any access type/command
		/// </summary> 
		GENERIC_SRC_BIT = 0x4,
		/// <summary> 
		/// Can write with and access type/command
		/// </summary> 
		GENERIC_DST_BIT = 0x8,
		COPY_SRC_BIT_KHR = COPY_SRC_BIT,
		COPY_DST_BIT_KHR = COPY_DST_BIT,
		GENERIC_SRC_BIT_KHR = GENERIC_SRC_BIT,
		GENERIC_DST_BIT_KHR = GENERIC_DST_BIT,
	}
}
