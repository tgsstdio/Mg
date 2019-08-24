using System;

namespace Magnesium
{
	[Flags]
	public enum MgExternalMemoryHandleTypeFlagBits : UInt32
	{
		OPAQUE_FD_BIT = 0x1,
		OPAQUE_WIN32_BIT = 0x2,
		OPAQUE_WIN32_KMT_BIT = 0x4,
		D3D11_TEXTURE_BIT = 0x8,
		D3D11_TEXTURE_KMT_BIT = 0x10,
		D3D12_HEAP_BIT = 0x20,
		D3D12_RESOURCE_BIT = 0x40,
		OPAQUE_FD_BIT_KHR = OPAQUE_FD_BIT,
		OPAQUE_WIN32_BIT_KHR = OPAQUE_WIN32_BIT,
		OPAQUE_WIN32_KMT_BIT_KHR = OPAQUE_WIN32_KMT_BIT,
		D3D11_TEXTURE_BIT_KHR = D3D11_TEXTURE_BIT,
		D3D11_TEXTURE_KMT_BIT_KHR = D3D11_TEXTURE_KMT_BIT,
		D3D12_HEAP_BIT_KHR = D3D12_HEAP_BIT,
		D3D12_RESOURCE_BIT_KHR = D3D12_RESOURCE_BIT,
		DMA_BUF_BIT_EXT = 0x200,
		ANDROID_HARDWARE_BUFFER_BIT_ANDROID = 0x400,
		HOST_ALLOCATION_BIT_EXT = 0x80,
		HOST_MAPPED_FOREIGN_MEMORY_BIT_EXT = 0x100,
	}
}
