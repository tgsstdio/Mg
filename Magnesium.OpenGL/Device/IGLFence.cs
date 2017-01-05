namespace Magnesium.OpenGL
{
    public interface IGLFence : IMgFence
    {
        bool IsSignalled { get; }
        void Reset();
        void BeginSync();
        bool IsReady(long timeInNanoSecs);
    }
}
