namespace Magnesium.OpenGL.Internals
{
    public struct GLCmdPipelineStencilWriteInfo
    {
        public MgStencilFaceFlagBits Face { get; internal set; }
        public uint WriteMask { get; internal set; }
    }
}