namespace Magnesium.OpenGL
{
    public interface IGLFutureDescriptorPoolEntrypoint
    {
        IGLFutureDescriptorPool CreatePool(MgDescriptorPoolCreateInfo createInfo);
    }
}
