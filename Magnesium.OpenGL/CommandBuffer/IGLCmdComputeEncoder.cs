namespace Magnesium.OpenGL.Internals
{
    public interface IGLCmdComputeEncoder
    {
        GLCmdComputeGrid AsGrid();

        void BindPipeline(IMgPipeline pipeline);
        void Clear();
        void Dispatch(uint x, uint y, uint z);
        void DispatchIndirect(IMgBuffer buffer, ulong offset);
    }
}