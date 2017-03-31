namespace Magnesium.OpenGL.DesktopGL
{
    public class GLContextVersion
    {
        public GLContextVersion(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
    }
}