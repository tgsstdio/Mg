using System.Collections.Generic;

namespace Magnesium.Utilities
{
    public class MgStorageBlockInfo
    {
        public MgBufferUsageFlagBits Usage { get; internal set; }
        public List<MgStorageBlockAttribute> Attributes { get; set; }
    }    
}
