﻿using System.Collections.Generic;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdEncoderItemCollection<TData>
	{
		public GLCmdEncoderItemCollection()
		{
			mItems = new List<TData>();
		}
		private readonly List<TData> mItems;

		public uint Push(TData item)
		{
			var count = (uint)mItems.Count;
			mItems.Add(item);
			return count;
		}

		public void Clear()
		{
			mItems.Clear();
		}

		public TData[] ToArray()
		{
			return mItems.ToArray();
		}
	}
}
