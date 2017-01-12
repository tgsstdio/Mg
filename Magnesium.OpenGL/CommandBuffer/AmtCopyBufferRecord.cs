namespace Magnesium.OpenGL
{
    public class AmtCopyBufferRecord
    {
        public int Destination { get; internal set; }
        public AmtCopyBufferRegionRecord[] Regions { get; internal set; }
        public int Source { get; internal set; }
    }
}