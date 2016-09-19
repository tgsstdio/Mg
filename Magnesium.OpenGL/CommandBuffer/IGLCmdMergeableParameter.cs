namespace Magnesium.OpenGL
{
	public interface IGLCmdMergeableParameter<TData>
	{
		TData Merge (TData delta);
	}

}

