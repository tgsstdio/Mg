using System;

namespace Magnesium
{
	[Flags]
	public enum MgExternalMemoryHandleTypeFlagBitsNV : UInt32
	{
		OPAQUE_WIN32_BIT_NV = 0x1,
		OPAQUE_WIN32_KMT_BIT_NV = 0x2,
		D3D11_IMAGE_BIT_NV = 0x4,
		D3D11_IMAGE_KMT_BIT_NV = 0x8,
	}
}
