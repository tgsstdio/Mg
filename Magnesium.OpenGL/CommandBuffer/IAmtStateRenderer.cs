namespace Magnesium.OpenGL
{
    public interface IAmtStateRenderer
    {
        void BeginRenderpass(AmtBeginRenderpassRecord record);
        void EndRenderpass();
        void BindPipeline(AmtBoundPipelineRecordInfo pipelineInfo);
        void SetStencilWriteMask(AmtPipelineStencilWriteInfo write);
    }
}