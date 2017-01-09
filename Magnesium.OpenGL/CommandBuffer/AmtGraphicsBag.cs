namespace Magnesium.OpenGL
{
    public class AmtGraphicsBag
    {
        public AmtEncoderItemCollection<AmtBeginRenderpassRecord> Renderpasses { get; set; }
        public AmtEncoderItemCollection<AmtBoundPipelineRecordInfo> Pipelines { get; internal set; }
        public AmtEncoderItemCollection<AmtPipelineStencilWriteInfo> StencilWrites { get; internal set; }

        public AmtGraphicsBag()
        {
            Renderpasses = new AmtEncoderItemCollection<AmtBeginRenderpassRecord>();
            Pipelines = new AmtEncoderItemCollection<AmtBoundPipelineRecordInfo>();
            StencilWrites = new AmtEncoderItemCollection<AmtPipelineStencilWriteInfo>();
        }

        public void Clear()
        {
            Renderpasses.Clear();
            Pipelines.Clear();
            StencilWrites.Clear();
        }
    }
}
