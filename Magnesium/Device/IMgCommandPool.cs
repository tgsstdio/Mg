namespace Magnesium
{
    public interface IMgCommandPool
	{
		void DestroyCommandPool(IMgDevice device, IMgAllocationCallbacks allocator);
		Result ResetCommandPool(IMgDevice device, MgCommandPoolResetFlagBits flags);
	}
}

