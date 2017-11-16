namespace Magnesium.OpenGL
{
    // Sits at shader cache level (run-time) 
    public interface IGLShaderTextureDescriptorCache
    {
        void Initialize();
        void Bind(IGLFutureDescriptorSet ds, GLDescriptorPoolResourceInfo resource);
    }
}
