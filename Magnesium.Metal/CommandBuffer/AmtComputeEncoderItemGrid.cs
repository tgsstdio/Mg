namespace Magnesium.Metal
{
	public class AmtComputeEncoderItemGrid
	{
		public AmtComputePipeline[] Pipelines { get; set;}
		public AmtDispatchEncoderState[] Dispatch { get; set;}
		public AmtDispatchIndirectEncoderState[] DispatchIndirect { get; set;}
	}
}
}