namespace Magnesium.OpenGL.Internals
{
    public struct AmtPipelineStencilWriteInfo
    {
        public MgStencilFaceFlagBits Face { get; internal set; }
        public uint WriteMask { get; internal set; }
    }
}