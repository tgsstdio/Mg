namespace Magnesium
{
    public interface IMgQueryPool
	{
		void DestroyQueryPool(IMgDevice device, MgAllocationCallbacks allocator);
	}
}

