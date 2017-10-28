namespace Magnesium.OpenGL
{
    public class GLOffscreenDeviceEntrypoint : IMgOffscreenDeviceEntrypoint
    {
        class GLNullOffscreenDeviceLocalMemory : IMgOffscreenDeviceLocalMemory
        {
            public void FreeMemory()
            {
             
            }

            public void Initialize(IMgImage offscreenImage)
            {
          
            }
        }

        public IMgOffscreenDeviceLocalMemory InitializeDeviceMemory(IMgGraphicsConfiguration configuration)
        {
            return new GLNullOffscreenDeviceLocalMemory();
        }
    }
}
