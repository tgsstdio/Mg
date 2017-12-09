using Magnesium;
using Magnesium.Utilities;

namespace OffscreenDemo
{
    public interface IMgRenderableElement
    {
        void Reserve(MgBlockAllocationList request);
        void Populate(IMgDevice device, MgOptimizedStorageContainer container, IMgCommandBuffer cmdBuf);
        void Build(MgCommandBuildOrder order, SimpleEffectPipeline pipeline);
        void Refresh(IMgDevice device, MgOptimizedStorageContainer container, IMgEffectFramework framework);
        void Setup(IMgDevice device, MgCommandBuildOrder order, MgOptimizedStorageContainer container);
    }
}
