using Magnesium.Utilities;
using System.Collections.Generic;

namespace OffscreenDemo
{
    public class MgBlockAllocationList
    {
        private List<MgBlockAllocationInfo> mItems;

        public MgBlockAllocationList()
        {
            mItems = new List<MgBlockAllocationInfo>();
        }

        public int Insert(MgBlockAllocationInfo info)
        {
            int count = mItems.Count;
            mItems.Add(info);
            return count;
        }

        public void Clear()
        {
            mItems.Clear();
        }
    }
}