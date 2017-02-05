using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLUniformBlockGroupCollator
	{
		readonly Dictionary<string, GLUniformBlockGroupInfo> mGroups;

		public GLUniformBlockGroupCollator()
		{
			mGroups = new Dictionary<string, GLUniformBlockGroupInfo>();
		}
		public void Add(GLUniformBlockInfo entry)
		{
			GLUniformBlockGroupInfo found;
			if (mGroups.TryGetValue(entry.Prefix, out found))
			{
				if (entry.X == 0 && entry.Y == 0 && entry.Z == 0)
				{
					found.BindingIndex = entry.BindingIndex;
				}

				found.ArrayStride = Math.Max(found.ArrayStride, entry.X + 1);
				found.HighestRow = Math.Max(found.HighestRow, entry.Y + 1);
				found.HighestLayer = Math.Max(found.HighestLayer, entry.Z + 1);
				found.Count += 1;
			}
			else
			{
				found = new GLUniformBlockGroupInfo
				{
					Prefix = entry.Prefix,
					BindingIndex = entry.BindingIndex,
					Count = 1,
					ArrayStride = entry.X + 1,
					HighestRow = entry.Y + 1,
					HighestLayer = entry.Z + 1,
				};
				mGroups.Add(found.Prefix, found);
			}
		}

		public SortedDictionary<uint, GLUniformBlockGroupInfo> Collate()
		{
			var sortedResults = new SortedDictionary<uint, GLUniformBlockGroupInfo>();
			foreach (var blockGroup in mGroups.Values)
			{
				blockGroup.MatrixStride = (blockGroup.ArrayStride * Math.Max(blockGroup.HighestRow, 1));
				blockGroup.CubeStride = (blockGroup.MatrixStride * Math.Max(blockGroup.HighestLayer, 1));
				sortedResults.Add(blockGroup.BindingIndex, blockGroup);
			}

			return sortedResults;
		}
	}
}