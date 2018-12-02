
namespace Magnesium
{
    public interface IMgEntrypoint
	{
		IMgAllocationCallbacks CreateAllocationCallbacks();
		MgResult CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance);
		MgResult EnumerateInstanceLayerProperties(out MgLayerProperties[] properties);
		MgResult EnumerateInstanceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties);
	}
}

