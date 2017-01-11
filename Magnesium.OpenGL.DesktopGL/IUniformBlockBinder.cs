using System;

namespace BirdNest.Core
{
	public interface IUniformBlockBinder : IMemoryUser
	{
		void Initialise();
		bool Reserve<TData, TReference>(params TReference[] objs)
			where TData : struct
			where TReference : class;
		void Update();
		void Setup(DisplayItem item);
		void Bind(DisplayItem item);
	}
}

