namespace Magnesium
{
    public interface IMgCommandPool
	{
		void DestroyCommandPool(IMgDevice device, IMgAllocationCallbacks allocator);
		MgResult ResetCommandPool(IMgDevice device, MgCommandPoolResetFlagBits flags);
	}
}

