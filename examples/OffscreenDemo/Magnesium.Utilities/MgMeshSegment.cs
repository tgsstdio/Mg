using System.Collections.Generic;

namespace Magnesium.Utilities
{
    public class MgMeshSegment
    {
        public MgBufferUsageFlagBits Usage { get; internal set; }
        public List<MgMeshSegmentAttribute> Attributes { get; set; }
    }    
}
