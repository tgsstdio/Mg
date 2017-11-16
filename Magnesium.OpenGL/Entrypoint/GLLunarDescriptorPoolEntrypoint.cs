namespace Magnesium.OpenGL
{
    public class GLLunarDescriptorPoolEntrypoint : IGLFutureDescriptorPoolEntrypoint
    {
        private readonly IGLLunarImageDescriptorEntrypoint mEntrypoint;
        public GLLunarDescriptorPoolEntrypoint(IGLLunarImageDescriptorEntrypoint entrypoint)
        {
            mEntrypoint = entrypoint;
        }

        public IGLFutureDescriptorPool CreatePool(MgDescriptorPoolCreateInfo createInfo)
        {
            return new GLLunarDescriptorPool(mEntrypoint, createInfo);
        }
    }
}
