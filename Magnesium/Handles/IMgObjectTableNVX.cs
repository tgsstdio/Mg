namespace Magnesium
{
    public interface IMgObjectTableNVX
    {
        void DestroyObjectTableNVX(IMgDevice device, IMgAllocationCallbacks pAllocator);
        MgResult RegisterObjectsNVX(IMgDevice device, MgObjectRegistrationNVX[] registrationObjects);
        MgResult UnregisterObjectsNVX(IMgDevice device, MgObjectRegistrationNVX[] registrationObjects);
    }
}
