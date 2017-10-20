using Magnesium;
using Magnesium.Utilities;

namespace OffscreenDemo
{
    public interface IScreenQuadPipeline
    {
        void Initialize(IMgGraphicsConfiguration configuration, IMgEffectFramework framework);
        void Reserve(MgBlockAllocationList slots);
        MgCommandBuildOrder GenerateBuildOrder(MgOptimizedStorage storage);
        void BuildCommandBuffers(object secondOrder);
        void Populate(MgOptimizedStorage storage, IMgGraphicsConfiguration configuration, IMgCommandBuffer copyCmd);
    }
}