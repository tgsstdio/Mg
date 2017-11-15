namespace Magnesium.OpenGL.UnitTests
{
    interface IMgEntrypointValidationLayer
    {
        void CreateAllocationCallbacks(IMgEntrypoint element);
        void CreateInstance(IMgEntrypoint element, MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator);
        void EnumerateInstanceLayerProperties(IMgEntrypoint element);
        void EnumerateInstanceExtensionProperties(IMgEntrypoint element, string layerName);
    }
}
