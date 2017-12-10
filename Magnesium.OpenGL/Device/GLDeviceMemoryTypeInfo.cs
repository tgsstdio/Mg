namespace Magnesium.OpenGL
{
    public class GLDeviceMemoryTypeInfo
    {
        public uint Index { get; set; }
        public uint MemoryTypeIndex { get; set; }
        public MgMemoryPropertyFlagBits MemoryPropertyFlags { get; set; }
        public bool IsHosted { get; set; }
        public uint Hint { get; set; }
    }
}
