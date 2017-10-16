using Magnesium;

namespace OffscreenDemo
{
    public interface IDemoApplication
    {
        void Initialize(IMgGraphicsConfiguration configuration);

        void Update();

        IMgSemaphore[] Render(IMgQueue queue, uint layerNo);
    }
}
