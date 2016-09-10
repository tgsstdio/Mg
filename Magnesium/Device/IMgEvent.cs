namespace Magnesium
{
    public interface IMgEvent
	{
		Result GetEventStatus(IMgDevice device);
		Result SetEvent(IMgDevice device);
		Result ResetEvent(IMgDevice device);

		void DestroyEvent(IMgDevice device, IMgAllocationCallbacks allocator);
	}
}

