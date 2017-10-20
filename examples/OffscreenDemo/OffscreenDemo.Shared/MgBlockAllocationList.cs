using Magnesium.Utilities;
using System.Collections.Generic;

namespace OffscreenDemo
{
    public class MgBlockAllocationList
    {
        private List<MgStorageBlockAllocationInfo> mItems;

        public MgBlockAllocationList()
        {
            mItems = new List<MgStorageBlockAllocationInfo>();
        }

        public int Insert(MgStorageBlockAllocationInfo info)
        {
            int count = mItems.Count;
            mItems.Add(info);
            return count;
        }

        public void Clear()
        {
            mItems.Clear();
        }

        public MgStorageBlockAllocationInfo[] ToArray()
        {
            return mItems.ToArray();
        }
    }
}