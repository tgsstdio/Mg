namespace Magnesium
{
    public interface IMgOffscreenDeviceEntrypoint
    {
        IMgOffscreenDeviceLocalMemory InitializeDeviceMemory(IMgGraphicsConfiguration configuration);
    }
}
