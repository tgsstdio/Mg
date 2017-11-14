namespace Magnesium.OpenGL.UnitTests
{
    // Sits at shader cache level (run-time) 
    interface IGLFutureShaderDescriptorCache
    {
        void Initialize();
        void Bind(IGLFutureDescriptorSet ds, GLDescriptorPoolResourceInfo resource);
    }
}
