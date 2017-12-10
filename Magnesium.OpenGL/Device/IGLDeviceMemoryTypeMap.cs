namespace Magnesium.OpenGL
{
    public interface IGLDeviceMemoryTypeMap
    {
        uint DetermineTypeIndex(GLDeviceMemoryTypeFlagBits category);
        GLDeviceMemoryTypeInfo[] MemoryTypes { get; }
    }
}
