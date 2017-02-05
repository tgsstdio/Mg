namespace Magnesium.OpenGL
{
    public interface IGLDescriptorSetLayout : IMgDescriptorSetLayout
    {
        GLUniformBinding[] Uniforms { get; }
    }
}