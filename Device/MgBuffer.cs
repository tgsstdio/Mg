using System;

namespace Magnesium
{
    public interface IMgBuffer
	{
		void DestroyBuffer(IMgDevice device, MgAllocationCallbacks allocator);
		Result BindBufferMemory(IMgDevice device, IMgDeviceMemory memory, UInt64 memoryOffset);
	}
}

