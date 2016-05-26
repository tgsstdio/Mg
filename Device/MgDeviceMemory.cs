using System;

namespace Magnesium
{
    public interface IMgDeviceMemory
	{
		void FreeMemory(IMgDevice device, MgAllocationCallbacks allocator);
		Result MapMemory(IMgDevice device, UInt64 offset, UInt64 size, UInt32 flags, out IntPtr ppData);
		void UnmapMemory(IMgDevice device);
	}
}

