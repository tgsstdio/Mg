
namespace Magnesium
{
    public interface IMgEntrypoint
	{
		IMgAllocationCallbacks CreateAllocationCallbacks();
		Result CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance);
		Result EnumerateInstanceLayerProperties(out MgLayerProperties[] properties);
		Result EnumerateInstanceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties);
	}
}

