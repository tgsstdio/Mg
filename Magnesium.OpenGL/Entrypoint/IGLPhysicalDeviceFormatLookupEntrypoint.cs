namespace Magnesium.OpenGL
{
    public interface IGLPhysicalDeviceFormatLookupEntrypoint
    {
        bool TryGetValue(MgFormat query, out MgFormatProperties properties);
    }
}