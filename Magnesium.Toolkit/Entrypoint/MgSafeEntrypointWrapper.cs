namespace Magnesium.Toolkit
{
    public class MgSafeEntrypointWrapper : IMgEntrypointWrapper
    {
        private readonly IMgEntrypoint mEntrypoint;
        public MgSafeEntrypointWrapper(IMgEntrypoint entrypoint)
        {
            mEntrypoint = entrypoint;
        }

        public IMgAllocationCallbacks CreateAllocationCallbacks()
        {
            return mEntrypoint.CreateAllocationCallbacks();
        }

        public MgResult CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance)
        {
            var result = mEntrypoint.CreateInstance(createInfo, allocator, out IMgInstance internalInstance);
            if (result != MgResult.SUCCESS)
            {
                instance = internalInstance;
                return result;
            }

            instance = new MgSafeInstance(internalInstance);
            return result;
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
