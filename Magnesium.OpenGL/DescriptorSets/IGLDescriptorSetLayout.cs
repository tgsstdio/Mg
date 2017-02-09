namespace Magnesium.OpenGL.Internals
{
    public interface IGLDescriptorSetLayout : IMgDescriptorSetLayout
    {
        GLUniformBinding[] Uniforms { get; }
    }
}