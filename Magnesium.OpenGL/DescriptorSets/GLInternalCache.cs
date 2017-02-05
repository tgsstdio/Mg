namespace Magnesium.OpenGL
{
	public class GLInternalCache
	{
		public int[] Strides { get; private set; }
		public GLInternalCacheBlockBinding[] BlockBindings { get; private set; }
		private readonly GLInternalCacheArrayMapper mArrayMapper;
		public GLInternalCache
		(
			IGLPipelineLayout pipelineLayout,
			GLUniformBlockEntry[] blockEntries,
			GLInternalCacheArrayMapper arrayMapper
		)
		{
			mArrayMapper = arrayMapper;
			SetupBlockBindings(blockEntries, mArrayMapper);
			SetupStrides(blockEntries, pipelineLayout, mArrayMapper);
		}

		void SetupStrides(
			GLUniformBlockEntry[] blockEntries
			, IGLPipelineLayout layout
			, GLInternalCacheArrayMapper arrayMapper)
		{
			Strides = new int[layout.NoOfBindingPoints];
			for (var i = 0; i < layout.NoOfBindingPoints; i += 1)
			{
				Strides[i] = 0;
			}

			foreach (var entry in blockEntries)
			{
				var arrayIndex = arrayMapper.CalculateArrayIndex(entry);
				Strides[arrayIndex] = entry.Stride;
			}
		}

		void SetupBlockBindings(GLUniformBlockEntry[] entries, GLInternalCacheArrayMapper arrayMapper)
		{
			BlockBindings = new GLInternalCacheBlockBinding[entries.Length];
			for (var i = 0; i < entries.Length; i += 1)
			{
				var entry = entries[i];
				BlockBindings[i] = new GLInternalCacheBlockBinding
				{
					BlockName = entry.BlockName,
					ActiveIndex = entry.ActiveIndex,
					BindingPoint = arrayMapper.CalculateArrayIndex(entry),
				};
			}
		}
	}
}
