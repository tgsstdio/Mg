using System;

namespace Magnesium
{
	[Flags]
	public enum MgExternalFenceHandleTypeFlagBits : UInt32
	{
		OPAQUE_FD_BIT = 0x1,
		OPAQUE_WIN32_BIT = 0x2,
		OPAQUE_WIN32_KMT_BIT = 0x4,
		SYNC_FD_BIT = 0x8,
		OPAQUE_FD_BIT_KHR = OPAQUE_FD_BIT,
		OPAQUE_WIN32_BIT_KHR = OPAQUE_WIN32_BIT,
		OPAQUE_WIN32_KMT_BIT_KHR = OPAQUE_WIN32_KMT_BIT,
		SYNC_FD_BIT_KHR = SYNC_FD_BIT,
	}
}
