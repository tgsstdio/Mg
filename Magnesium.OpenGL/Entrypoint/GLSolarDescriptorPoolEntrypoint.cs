namespace Magnesium.OpenGL
{
    public class GLSolarDescriptorPoolEntrypoint : IGLFutureDescriptorPoolEntrypoint
    {
        public IGLFutureDescriptorPool CreatePool(MgDescriptorPoolCreateInfo createInfo)
        {
            return new GLSolarDescriptorPool(createInfo);
        }
    }
}
