using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLCmdBufferStore<TData> : IGLCmdBufferStoreResettable, IGLCmdBufferStore<TData>
	{
		public GLCmdBufferStore ()
		{
			mItems = new List<TData>();
		}

		public int Count {
			get {
				return mItems.Count;
			}
		}

		private readonly List<TData> mItems;

		public TData At(int index)
		{
			return mItems [index];
		}

		public void Add(TData item)
		{
			mItems.Add (item);
		}

		public int? LastIndex()
		{
			int? result = null;
			if (mItems.Count > 0)
				result = (mItems.Count - 1);
			return result;
		}

		public bool LastValue(ref TData item)
		{
			if (mItems.Count == 0)
			{				
				return false;
			}
			else
			{
				item = mItems[mItems.Count - 1];
				return true;
			}
		}

		public void Clear()
		{
			mItems.Clear ();
		}
	}
}

