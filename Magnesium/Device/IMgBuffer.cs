using System;

namespace Magnesium
{
    public interface IMgBuffer
	{
		global::Metal.IMTLBuffer Buffer { get; set; }

		void DestroyBuffer(IMgDevice device, IMgAllocationCallbacks allocator);
		Result BindBufferMemory(IMgDevice device, IMgDeviceMemory memory, UInt64 memoryOffset);
	}
}

