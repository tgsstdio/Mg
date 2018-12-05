using System;

namespace Magnesium
{
	[Flags]
	public enum MgFenceImportFlagBits : UInt32
	{
		TEMPORARY_BIT = 0x1,
		TEMPORARY_BIT_KHR = TEMPORARY_BIT,
	}
}
