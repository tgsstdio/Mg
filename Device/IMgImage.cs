using System;

namespace Magnesium
{
    public interface IMgImage
	{
		Result BindImageMemory(IMgDevice device, IMgDeviceMemory memory, UInt64 memoryOffset);		
		void DestroyImage(IMgDevice device, MgAllocationCallbacks allocator);
	}
}

