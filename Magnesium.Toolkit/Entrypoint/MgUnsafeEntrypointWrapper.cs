namespace Magnesium.Toolkit
{
    public class MgUnsafeEntrypointWrapper : IMgEntrypointWrapper
    {
        private readonly IMgEntrypoint mEntrypoint;
        public MgUnsafeEntrypointWrapper(IMgEntrypoint entrypoint)
        {
            mEntrypoint = entrypoint;
        }

        public IMgAllocationCallbacks CreateAllocationCallbacks()
        {
            return mEntrypoint.CreateAllocationCallbacks();
        }

        public MgResult CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance)
        {
            return mEntrypoint.CreateInstance(createInfo, allocator, out instance);
        }

        public MgResult EnumerateInstanceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
        {
            return mEntrypoint.EnumerateInstanceExtensionProperties(layerName, out pProperties);
        }

        public MgResult EnumerateInstanceLayerProperties(out MgLayerProperties[] properties)
        {
            return mEntrypoint.EnumerateInstanceLayerProperties(out properties);
        }
    }
}
