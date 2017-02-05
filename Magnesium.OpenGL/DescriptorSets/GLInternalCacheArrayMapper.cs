using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLInternalCacheArrayMapper
	{
		private readonly IGLPipelineLayout mLayout;
		private readonly SortedDictionary<uint, GLUniformBlockGroupInfo> mGroups;

		public GLInternalCacheArrayMapper(IGLPipelineLayout layout, GLUniformBlockEntry[] blockEntries)
		{
			mLayout = layout;

			var collator = new GLUniformBlockGroupCollator();
			foreach (var entry in blockEntries)
			{
				collator.Add(entry.Token);
			}

			var groups = collator.Collate();
			mGroups = groups;
		}

		public uint CalculateArrayIndex(GLUniformBlockEntry entry)
		{
			uint bindingPoint = 0U;

			var mapGroup = mGroups[entry.Token.BindingIndex];

			// ROW-ORDER 
			bindingPoint += entry.Token.X;
			bindingPoint += (mapGroup.ArrayStride * entry.Token.Y);
			bindingPoint += (mapGroup.MatrixStride * entry.Token.Z);

			var arrayOffset = mLayout.Ranges[entry.Token.BindingIndex];
			bindingPoint += arrayOffset.First;
			return bindingPoint;
		}
	}
}
