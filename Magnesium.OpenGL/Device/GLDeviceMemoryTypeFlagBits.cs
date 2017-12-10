namespace Magnesium.OpenGL
{
    public enum GLDeviceMemoryTypeFlagBits: uint
    {
        INDIRECT = 1 << 0,
        IMAGE = 1 << 1,
        VERTEX = 1 << 2,
        INDEX = 1 << 3,
        UNIFORM = 1 << 4,
        TRANSFER_SRC = 1 << 5,
        TRANSFER_DST = 1 << 6,
        STORAGE = 1 << 7,
    }
}
