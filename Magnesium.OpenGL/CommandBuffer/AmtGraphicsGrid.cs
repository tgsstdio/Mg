namespace Magnesium.OpenGL
{
    public class AmtGraphicsGrid
    {
        public AmtBeginRenderpassRecord[] Renderpasses { get; set; }
        public AmtBoundPipelineRecordInfo[] Pipelines { get; set; }
        public AmtPipelineStencilWriteInfo[] StencilWrites { get; set; }
        public GLCmdViewportParameter[] Viewports { get; set; }
        public GLCmdScissorParameter[] Scissors { get; set; }
        public GLCmdDepthBiasParameter[] DepthBias { get; set; }
        public float[] LineWidths { get; set; }
        public GLCmdDepthBoundsParameter[] DepthBounds { get; set; }
        public MgColor4f[] BlendConstants { get; set; }
        public GLCmdVertexBufferObject[] VAOs { get; set; }
        public AmtGLStencilFunctionInfo[] StencilFunctions { get; set; }
    }
}