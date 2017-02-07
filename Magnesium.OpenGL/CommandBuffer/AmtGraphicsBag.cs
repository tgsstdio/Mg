namespace Magnesium.OpenGL.Internals
{
    public class AmtGraphicsBag
    {
        public AmtEncoderItemCollection<AmtBeginRenderpassRecord> Renderpasses { get; set; }
        public AmtEncoderItemCollection<AmtBoundPipelineRecordInfo> Pipelines { get; internal set; }
        public AmtEncoderItemCollection<AmtPipelineStencilWriteInfo> StencilWrites { get; internal set; }
        public AmtEncoderItemCollection<GLCmdViewportParameter> Viewports { get; internal set; }
        public AmtEncoderItemCollection<GLCmdScissorParameter> Scissors { get; internal set; }
        public AmtEncoderItemCollection<GLCmdDepthBiasParameter> DepthBias { get; internal set; }
        public AmtEncoderItemCollection<float> LineWidths { get; internal set; }
        public AmtEncoderItemCollection<GLCmdDepthBoundsParameter> DepthBounds { get; internal set; }
        public AmtEncoderItemCollection<MgColor4f> BlendConstants { get; internal set; }
        public AmtEncoderItemCollection<GLCmdVertexBufferObject> VAOs { get; internal set; }
        public AmtEncoderItemCollection<AmtGLStencilFunctionInfo> StencilFunctions { get; internal set; }
        public AmtEncoderItemCollection<GLCmdInternalDraw> Draws { get; internal set; }
        public AmtEncoderItemCollection<GLCmdInternalDrawIndexed> DrawIndexeds { get; internal set; }
        public AmtEncoderItemCollection<GLCmdInternalDrawIndirect> DrawIndirects { get; internal set; }
        public AmtEncoderItemCollection<GLCmdInternalDrawIndexedIndirect> DrawIndexedIndirects { get; internal set; }

        public AmtGraphicsBag()
        {
            Renderpasses = new AmtEncoderItemCollection<AmtBeginRenderpassRecord>();
            Pipelines = new AmtEncoderItemCollection<AmtBoundPipelineRecordInfo>();
            StencilWrites = new AmtEncoderItemCollection<AmtPipelineStencilWriteInfo>();
            Viewports = new AmtEncoderItemCollection<GLCmdViewportParameter>();
            Scissors = new AmtEncoderItemCollection<GLCmdScissorParameter>();
            DepthBias = new AmtEncoderItemCollection<GLCmdDepthBiasParameter>();
            LineWidths = new AmtEncoderItemCollection<float>();
            DepthBounds = new AmtEncoderItemCollection<GLCmdDepthBoundsParameter>();
            BlendConstants = new AmtEncoderItemCollection<MgColor4f>();
            VAOs = new AmtEncoderItemCollection<GLCmdVertexBufferObject>();
            StencilFunctions = new AmtEncoderItemCollection<AmtGLStencilFunctionInfo>();
            Draws = new AmtEncoderItemCollection<GLCmdInternalDraw>();
            DrawIndexeds = new AmtEncoderItemCollection<GLCmdInternalDrawIndexed>();
            DrawIndirects = new AmtEncoderItemCollection<GLCmdInternalDrawIndirect>();
            DrawIndexedIndirects = new AmtEncoderItemCollection<GLCmdInternalDrawIndexedIndirect>();
        }

        public void Clear()
        {
            Renderpasses.Clear();
            Pipelines.Clear();
            StencilWrites.Clear();
            Viewports.Clear();
            Scissors.Clear();
            DepthBias.Clear();
            LineWidths.Clear();
            DepthBounds.Clear();
            BlendConstants.Clear();
            VAOs.Clear();
            StencilFunctions.Clear();
            Draws.Clear();
            DrawIndexeds.Clear();
            DrawIndirects.Clear();
            DrawIndexedIndirects.Clear();
        }
    }
}
