namespace Magnesium.OpenGL.DesktopGL
{
    public class GLNullOffscreenDeviceLocalMemory : IMgOffscreenDeviceLocalMemory
    {
        public void FreeMemory()
        {
            
        }

        public void Initialize(IMgImage offscreenImage)
        {
            // NO NEED - OPENGL uses texture storage as device local memory
        }
    }
}
