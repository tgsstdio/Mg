using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class DynamicStateItemStore<TData> where TData : IEquatable<TData>
	{
		public List<TData> Items { get; private set; }

		GLGraphicsPipelineDynamicStateFlagBits mOverrideFlag;

		readonly Func<GLCmdDrawCommand, int?> mIndexFunc;

		readonly Func<IGLGraphicsPipeline, TData> mMemberFunc;

		readonly Func<IGLCmdBufferRepository, int, TData> mFetch;

		public DynamicStateItemStore (
			GLGraphicsPipelineDynamicStateFlagBits flag,
			Func<GLCmdDrawCommand, int?> getLocalIndex, 
			Func<IGLCmdBufferRepository, int, TData> fetchOverride,
			Func<IGLGraphicsPipeline, TData> usePipelineDefault
		)
		{
			mOverrideFlag = flag;
			Items = new List<TData> ();
			mIndexFunc = getLocalIndex;
			mMemberFunc = usePipelineDefault;
			mFetch = fetchOverride;
		}

		public int Count 
		{
			get{
				return Items.Count;
			}
		}

		bool CanOverride(GLCmdDrawCommand drawCommand, IGLGraphicsPipeline pipeline)
		{
			return mIndexFunc (drawCommand).HasValue && ((pipeline.DynamicsStates & mOverrideFlag) == mOverrideFlag);
		}

		TData FetchOverride (IGLCmdBufferRepository repo, int? localIndex)
		{
			return mFetch (repo, localIndex.Value);
		}

		TData UsePipelineDefault (IGLGraphicsPipeline pipeline)
		{
			return mMemberFunc (pipeline);
		}

		public byte Extract (IGLCmdBufferRepository repo, IGLGraphicsPipeline pipeline, GLCmdDrawCommand drawCommand)
		{
			if (pipeline == null)
			{	
				throw new ArgumentNullException ("pipeline");
			}

			int? localIndex = mIndexFunc (drawCommand);

			TData currentValue = CanOverride (drawCommand, pipeline)
				? FetchOverride (repo, localIndex)
				: UsePipelineDefault (pipeline);

			// NONE EXISTS

			var count = Items.Count;

			if (count == 0)
			{
				// USE DEFAULT
				Items.Add (currentValue);
				return 0;
			}
			else
			{	
				var topIndex = count - 1;
				var lastValue = Items [topIndex];

				if (currentValue.Equals (lastValue))
				{
					return (byte)topIndex;
				}
				else
				{
					Items.Add (currentValue);
					return (byte)count;
				}
			}	
		}
	}
}

