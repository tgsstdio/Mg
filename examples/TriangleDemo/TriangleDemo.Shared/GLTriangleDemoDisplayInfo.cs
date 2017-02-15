using Magnesium;

namespace TriangleDemo
{
    public class GLTriangleDemoDisplayInfo : ITriangleDemoDisplayInfo
    {
        public MgFormat Color
        {
            get;
            set;
        }

        public MgFormat Depth
        {
            get;
            set;
        }
    }
}
