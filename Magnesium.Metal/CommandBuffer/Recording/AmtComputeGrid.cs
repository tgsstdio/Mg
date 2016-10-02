namespace Magnesium.Metal
{
	public class AmtComputeGrid
	{
		public AmtComputePipeline[] Pipelines { get; set;}
		public AmtDispatchRecord[] Dispatch { get; set;}
		public AmtDispatchIndirectRecord[] DispatchIndirect { get; set;}
	}
}