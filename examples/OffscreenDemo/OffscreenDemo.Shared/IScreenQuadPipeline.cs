using Magnesium;

namespace OffscreenDemo
{
    public interface IScreenQuadPipeline
    {
        void Initialize(IMgGraphicsConfiguration configuration, MgOffscreenGraphicDevice offscreen);
    }
}