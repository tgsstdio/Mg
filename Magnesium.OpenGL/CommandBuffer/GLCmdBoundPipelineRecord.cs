namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBoundPipelineRecordInfo
    {
        public IGLGraphicsPipeline Pipeline { get; set; }
        public MgColor4f BlendConstants { get; internal set; }
        public GLCmdDepthBiasParameter DepthBias { get; internal set; }
        public GLCmdDepthBoundsParameter DepthBounds { get; internal set; }
        public float LineWidth { get; internal set; }
        public GLCmdScissorParameter Scissors { get; internal set; }
        public GLCmdViewportParameter Viewports { get; internal set; }
        internal GLCmdStencilFunctionInfo BackStencilInfo { get; set; }
        internal GLCmdStencilFunctionInfo FrontStencilInfo { get; set; }
        public uint FrontStencilWriteMask { get; internal set; }
        public uint BackStencilWriteMask { get; internal set; }
    }
}