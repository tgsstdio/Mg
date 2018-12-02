namespace Magnesium
{
    public interface IMgEvent
	{
		MgResult GetEventStatus(IMgDevice device);
		MgResult SetEvent(IMgDevice device);
		MgResult ResetEvent(IMgDevice device);

		void DestroyEvent(IMgDevice device, IMgAllocationCallbacks allocator);
	}
}

