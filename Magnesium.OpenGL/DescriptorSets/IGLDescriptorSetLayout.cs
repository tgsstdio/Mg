namespace Magnesium.OpenGL
{
    internal interface IGLDescriptorSetLayout : IMgDescriptorSetLayout
    {
        GLUniformBinding[] Uniforms { get; }
    }
}