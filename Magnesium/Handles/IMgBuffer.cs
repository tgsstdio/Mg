using System;

namespace Magnesium
{
    public interface IMgBuffer
	{
		void DestroyBuffer(IMgDevice device, IMgAllocationCallbacks allocator);
		MgResult BindBufferMemory(IMgDevice device, IMgDeviceMemory memory, UInt64 memoryOffset);
	}
}

