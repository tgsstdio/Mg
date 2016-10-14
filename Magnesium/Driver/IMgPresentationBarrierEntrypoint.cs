namespace Magnesium
{
	public interface IMgPresentationBarrierEntrypoint
	{
		void SubmitPrePresentBarrier(IMgCommandBuffer prePresent, IMgImage image);
		void SubmitPostPresentBarrier(IMgCommandBuffer postPresent, IMgImage image);
	}
}