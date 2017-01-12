namespace Magnesium.OpenGL
{
    public class AmtBlitRecording
    {
        public AmtBlitGrid Grid { get; internal set; }
        public IGLBlitOperationEntrypoint Entrypoint { get; set; }
    }
}