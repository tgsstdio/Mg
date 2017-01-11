using System;

namespace BirdNest.Core
{
	public interface IUniformBlockSetter<TBlock, TReference>
		where TBlock : struct
		where TReference : class
	{
		void Set(ref TBlock dest, TReference src);
	}
}

