namespace Magnesium
{
    public class MgSubpassTransactionsInfo
    {
        public uint Subpass { get; set; }
        public uint[] Loads { get; set; }
        public uint[] Stores { get; set; }
        public uint[] ColorAttachments { get; internal set; }
        public uint? DepthAttachment { get; internal set; }
    }
}
