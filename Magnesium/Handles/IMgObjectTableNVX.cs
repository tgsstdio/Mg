namespace Magnesium
{
    public interface IMgObjectTableNVX
    {
        void DestroyObjectTableNVX(IMgDevice device, IMgAllocationCallbacks pAllocator);
        MgResult RegisterObjectsNVX(IMgDevice device, MgObjectTableEntryIndexNYX[] registrationObjects);
        MgResult UnregisterObjectsNVX(IMgDevice device, MgObjectTableEntryIndexNYX[] registrationObjects);
    }
}
