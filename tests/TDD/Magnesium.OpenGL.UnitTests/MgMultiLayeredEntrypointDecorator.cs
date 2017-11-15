namespace Magnesium.OpenGL.UnitTests
{
    class MgMultiLayeredEntrypointDecorator : IMgEntrypoint
    {
        private IMgEntrypoint mElement;
        private IMgEntrypointValidationLayer[] mEntrypointLayers;
        private IMgDecoratorFactory mFactory;

        public MgMultiLayeredEntrypointDecorator(
            IMgEntrypoint implementation,
            IMgEntrypointValidationLayer[] entrypointLayers,
            IMgDecoratorFactory factory
        )
        {
            mElement = implementation;
            mEntrypointLayers = entrypointLayers;
            mFactory = factory;
        }

        public IMgAllocationCallbacks CreateAllocationCallbacks()
        {
           if (mEntrypointLayers != null)
           {
                foreach(var layer in mEntrypointLayers)
                {
                    if (layer != null)
                        layer.CreateAllocationCallbacks(mElement);
                }
           }
           return mElement.CreateAllocationCallbacks();
        }

        public Result CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance)
        {
            if (mEntrypointLayers != null)
            {
                foreach (var layer in mEntrypointLayers)
                {
                    if (layer != null)
                        layer.CreateInstance(mElement, createInfo, allocator);
                }
            }
            var result = mElement.CreateInstance(createInfo, allocator, out IMgInstance internalInstance);
            if (result != Result.SUCCESS)
            {
                instance = internalInstance;
                return result;
            }
            else
            {
                // wrap instance
                instance = mFactory.WrapInstance(internalInstance);
                return result;
            }
        }

        public Result EnumerateInstanceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
        {
            if (mEntrypointLayers != null)
            {
                foreach (var layer in mEntrypointLayers)
                {
                    if (layer != null)
                        layer.EnumerateInstanceExtensionProperties(mElement, layerName);
                }
            }
            return mElement.EnumerateInstanceExtensionProperties(layerName, out pProperties);
        }

        public Result EnumerateInstanceLayerProperties(out MgLayerProperties[] properties)
        {
            if (mEntrypointLayers != null)
            {
                foreach (var layer in mEntrypointLayers)
                {
                    if (layer != null)
                        layer.EnumerateInstanceLayerProperties(mElement);
                }
            }
            return mElement.EnumerateInstanceLayerProperties(out properties);
        }
    }
}
