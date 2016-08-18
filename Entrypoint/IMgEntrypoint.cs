
namespace Magnesium
{
    public interface IMgEntrypoint
	{
		Result CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance);
		Result EnumerateInstanceLayerProperties(out MgLayerProperties[] properties);
		Result EnumerateInstanceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties);
	}
}

