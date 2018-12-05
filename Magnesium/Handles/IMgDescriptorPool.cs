using System;

namespace Magnesium
{
    public interface IMgDescriptorPool
	{
		void DestroyDescriptorPool(IMgDevice device, IMgAllocationCallbacks allocator);
		MgResult ResetDescriptorPool(IMgDevice device, UInt32 flags);
	}
}

