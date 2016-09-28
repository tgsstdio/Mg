using System;
namespace Magnesium.Metal
{
	public class AmtComputeEncoderItemBag
	{
		public AmtComputeEncoderItemBag()
		{
			Pipelines = new AmtEncoderItemCollection<AmtComputePipeline>();
			Dispatch = new AmtEncoderItemCollection<AmtDispatchEncoderState>();
			DispatchIndirect = new AmtEncoderItemCollection<AmtDispatchIndirectEncoderState>();
		}

		public void Clear()
		{
			Pipelines.Clear();
			Dispatch.Clear();
			DispatchIndirect.Clear();
		}

		public AmtEncoderItemCollection<AmtComputePipeline> Pipelines { get; private set;}
		public AmtEncoderItemCollection<AmtDispatchEncoderState> Dispatch { get; private set;}
		public AmtEncoderItemCollection<AmtDispatchIndirectEncoderState> DispatchIndirect { get; private set; }
	}
}
