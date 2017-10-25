namespace Magnesium
{
    public class MgDefaultOffscreenDeviceEntrypoint : IMgOffscreenDeviceEntrypoint
    {       
        public IMgOffscreenDeviceLocalMemory InitializeDeviceMemory(IMgGraphicsConfiguration configuration)
        {
            return new MgOffscreenDeviceLocalMemory(configuration);            
        }
    }
}
