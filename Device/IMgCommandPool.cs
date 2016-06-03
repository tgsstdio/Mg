namespace Magnesium
{
    public interface IMgCommandPool
	{
		void DestroyCommandPool(IMgDevice device, MgAllocationCallbacks allocator);
		Result ResetCommandPool(IMgDevice device, MgCommandPoolResetFlagBits flags);
	}
}

