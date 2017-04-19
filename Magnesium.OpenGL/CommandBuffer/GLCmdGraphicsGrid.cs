using System;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdGraphicsGrid : IDisposable
    {
        public GLCmdBeginRenderpassRecord[] Renderpasses { get; set; }
        public GLCmdBoundPipelineRecordInfo[] Pipelines { get; set; }
        public GLCmdPipelineStencilWriteInfo[] StencilWrites { get; set; }
        public GLCmdViewportParameter[] Viewports { get; set; }
        public GLCmdScissorParameter[] Scissors { get; set; }
        public GLCmdDepthBiasParameter[] DepthBias { get; set; }
        public float[] LineWidths { get; set; }
        public GLCmdDepthBoundsParameter[] DepthBounds { get; set; }
        public MgColor4f[] BlendConstants { get; set; }
        public GLCmdVertexBufferObject[] VAOs { get; set; }
        public GLCmdStencilFunctionInfo[] StencilFunctions { get; set; }
        public GLCmdInternalDraw[] Draws { get; set; }
        public GLCmdInternalDrawIndexed[] DrawIndexeds { get; set; }
        public GLCmdInternalDrawIndirect[] DrawIndirects { get; set; }
        public GLCmdInternalDrawIndexedIndirect[] DrawIndexedIndirects { get; set; }
        public GLCmdDescriptorSetParameter[] DescriptorSets { get; set; }

        private bool mIsDisposed = false;
        public void Dispose()
        {
            if (mIsDisposed)
                return;

            if (VAOs != null)
            {
                foreach (var vao in VAOs)
                {
                    if (vao != null)
                    {
                        vao.Dispose();
                    }
                }
            }

            mIsDisposed = true;
        }
    }
}