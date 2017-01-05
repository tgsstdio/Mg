namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLFullFenceEntrypoint : IGLFenceEntrypoint
    {
        public IGLFence CreateFence()
        {
            return new GLFullFence();
        }
    }
}
