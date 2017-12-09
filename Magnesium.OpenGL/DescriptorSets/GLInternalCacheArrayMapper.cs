using System.Collections.Generic;

namespace Magnesium.OpenGL.Internals
{
	public class GLInternalCacheArrayMapper
	{
		private readonly IGLPipelineLayout mLayout;
		private readonly SortedDictionary<int, GLUniformBlockGroupInfo> mGroups;
        private readonly GLUniformBlockGroupCollator mCollator;

        public GLInternalCacheArrayMapper(IGLPipelineLayout layout, GLUniformBlockEntry[] blockEntries)
        {
            mLayout = layout;

            mCollator = new GLUniformBlockGroupCollator();
            foreach (var entry in blockEntries)
            {
                mCollator.Add(entry.Token);
            }

            mGroups = mCollator.Collate();

            DeriveFirstBindings(blockEntries);
        }

        private void DeriveFirstBindings(GLUniformBlockEntry[] blockEntries)
        {
            foreach (var entry in blockEntries)
            {
                GLUniformBlockGroupInfo found;
                if (mCollator.Prefixes.TryGetValue(entry.Token.Prefix, out found))
                {
                    entry.FirstBinding = found.FirstBinding;
                }
            }
        }

        public int CalculateArrayIndex(GLUniformBlockEntry entry)
		{
			int bindingPoint = 0;

			var mapGroup = mGroups[entry.FirstBinding];

			// ROW-ORDER 
			bindingPoint += entry.Token.X;
			bindingPoint += (mapGroup.ArrayStride * entry.Token.Y);
			bindingPoint += (mapGroup.MatrixStride * entry.Token.Z);

			var arrayOffset = mLayout.Ranges[entry.FirstBinding];
			bindingPoint += (int) arrayOffset.Binding;
			return bindingPoint;
		}
	}
}
