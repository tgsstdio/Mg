namespace Magnesium.OpenGL
{
    public class AmtGraphicsGrid
    {
        public AmtBeginRenderpassRecord[] Renderpasses { get; set; }
        public AmtBoundPipelineRecordInfo[] Pipelines { get; set; }
        public AmtPipelineStencilWriteInfo[] StencilWrites { get; set; }
    }
}