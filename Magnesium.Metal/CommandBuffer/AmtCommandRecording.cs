
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtCommandRecording
	{
		public IMTLCommandBuffer CommandBuffer { get; set;}
		public AmtCommandEncoderInstruction[] Instructions { get; set; }
		public void Play()
		{
			foreach (var step in Instructions)
			{
				step.Operation(this, step.Index);
			}
		}

		public AmtGraphicsRecording Graphics { get; set; }
		public AmtBlitRecording Blit { get; set; }
		public AmtComputeRecording Compute { get; set; }

		public void BeginGraphics(IMTLCommandBuffer buffer, MTLRenderPassDescriptor descriptor)
		{
			Debug.Assert(Graphics != null);
			Graphics.Encoder = buffer.CreateRenderCommandEncoder(descriptor);
		}

		public void EndGraphics()
		{
			Debug.Assert(Graphics != null);
			Debug.Assert(Graphics.Encoder != null);
			Graphics.Encoder.EndEncoding();
			//Graphics.Encoder = null;
		}

		public void BeginCompute(IMTLCommandBuffer buffer)
		{
			Debug.Assert(Compute != null);
			Compute.Encoder = buffer.ComputeCommandEncoder;
		}

		public void EndCompute()
		{
			Debug.Assert(Compute != null);
			Debug.Assert(Compute.Encoder != null);
			Compute.Encoder.EndEncoding();
		}
	}
}
