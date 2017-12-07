using Magnesium;

namespace OffscreenDemo
{
    public interface IMgPipelineSeed
    {
        IMgDescriptorSetLayout SetupDescriptorSetLayout(IMgDevice device);
        IMgDescriptorPool SetupDescriptorPool(IMgDevice device);
        IMgPipeline BuildPipeline(
            IMgDevice device, 
            IMgPipelineLayout layout, 
            IMgEffectFramework framework);
    }
}
