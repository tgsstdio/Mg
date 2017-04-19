namespace Magnesium.OpenGL.Internals
{
    public class GLCmdGraphicsBag
    {
        public GLCmdEncoderItemCollection<GLCmdBeginRenderpassRecord> Renderpasses { get; set; }
        public GLCmdEncoderItemCollection<GLCmdBoundPipelineRecordInfo> Pipelines { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdPipelineStencilWriteInfo> StencilWrites { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdViewportParameter> Viewports { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdScissorParameter> Scissors { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdDepthBiasParameter> DepthBias { get; internal set; }
        public GLCmdEncoderItemCollection<float> LineWidths { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdDepthBoundsParameter> DepthBounds { get; internal set; }
        public GLCmdEncoderItemCollection<MgColor4f> BlendConstants { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdVertexBufferObject> VAOs { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdStencilFunctionInfo> StencilFunctions { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdInternalDraw> Draws { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdInternalDrawIndexed> DrawIndexeds { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdInternalDrawIndirect> DrawIndirects { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdInternalDrawIndexedIndirect> DrawIndexedIndirects { get; internal set; }
        public GLCmdEncoderItemCollection<GLCmdDescriptorSetParameter> DescriptorSets { get; internal set; }

        public GLCmdGraphicsBag()
        {
            Renderpasses = new GLCmdEncoderItemCollection<GLCmdBeginRenderpassRecord>();
            Pipelines = new GLCmdEncoderItemCollection<GLCmdBoundPipelineRecordInfo>();
            StencilWrites = new GLCmdEncoderItemCollection<GLCmdPipelineStencilWriteInfo>();
            Viewports = new GLCmdEncoderItemCollection<GLCmdViewportParameter>();
            Scissors = new GLCmdEncoderItemCollection<GLCmdScissorParameter>();
            DepthBias = new GLCmdEncoderItemCollection<GLCmdDepthBiasParameter>();
            LineWidths = new GLCmdEncoderItemCollection<float>();
            DepthBounds = new GLCmdEncoderItemCollection<GLCmdDepthBoundsParameter>();
            BlendConstants = new GLCmdEncoderItemCollection<MgColor4f>();
            VAOs = new GLCmdEncoderItemCollection<GLCmdVertexBufferObject>();
            StencilFunctions = new GLCmdEncoderItemCollection<GLCmdStencilFunctionInfo>();
            Draws = new GLCmdEncoderItemCollection<GLCmdInternalDraw>();
            DrawIndexeds = new GLCmdEncoderItemCollection<GLCmdInternalDrawIndexed>();
            DrawIndirects = new GLCmdEncoderItemCollection<GLCmdInternalDrawIndirect>();
            DrawIndexedIndirects = new GLCmdEncoderItemCollection<GLCmdInternalDrawIndexedIndirect>();
            DescriptorSets = new GLCmdEncoderItemCollection<GLCmdDescriptorSetParameter>();
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
            DescriptorSets.Clear();
        }
    }
}
