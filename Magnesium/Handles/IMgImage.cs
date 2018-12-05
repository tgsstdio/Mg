using System;

namespace Magnesium
{
    public interface IMgImage
	{
		MgResult BindImageMemory(IMgDevice device, IMgDeviceMemory memory, UInt64 memoryOffset);		
		void DestroyImage(IMgDevice device, IMgAllocationCallbacks allocator);
	}
}

