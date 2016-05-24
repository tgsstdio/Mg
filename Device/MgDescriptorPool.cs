using System;

namespace Magnesium
{
    public interface IMgDescriptorPool
	{
		void DestroyDescriptorPool(IMgDevice device, MgAllocationCallbacks allocator);
		Result ResetDescriptorPool(IMgDevice device, UInt32 flags);
	}
}

