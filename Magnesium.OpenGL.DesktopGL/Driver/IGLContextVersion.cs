using OpenTK.Graphics;

namespace Magnesium.OpenGL.DesktopGL
{
    public class GLContextVersion
    {    
        public GLContextVersion(GraphicsContextFlags flags, int major, int minor)
        {
            Flags = flags;
            Major = major;
            Minor = minor;
        }

        public GraphicsContextFlags Flags { get; private set; }
        public int Major { get; private set; }
        public int Minor { get; private set; }
    }
}