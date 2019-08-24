namespace Magnesium.Toolkit
{
    public interface IMgOffscreenDeviceEntrypoint
    {
        IMgOffscreenDeviceLocalMemory InitializeDeviceMemory(IMgGraphicsConfiguration configuration);
    }
}
