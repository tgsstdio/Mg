namespace Magnesium.Toolkit
{
    public interface IMgPresentationLayer
	{
		/// <summary>
		/// Begins the draw.
		/// </summary>
		/// <returns>The index of the target buffer</returns>
		/// <param name = "postPresent"> for setuping the pipeline barrier </param>
		/// <param name="presentComplete">Signal semaphore of the previous frame has been presented. </param>
		uint BeginDraw (IMgCommandBuffer postPresent, IMgSemaphore presentComplete);

		/// <summary>
		/// Begins the draw.
		/// </summary>
		/// <returns>The index of the target buffer</returns>
		/// <param name = "postPresent"> for setuping the pipeline barrier </param>
		/// <param name="presentComplete">Signal semaphore of the previous frame has been presented. </param>
		/// <param name="timeout"> Timeout in nanoseconds. </param>
		uint BeginDraw (IMgCommandBuffer postPresent, IMgSemaphore presentComplete, ulong timeout);

		/// <summary>
		/// Ends the draw.
		/// </summary>
		/// <param name="nextImage">Next image.</param>
		/// <param name = "prePresent"> for setuping the pipeline barrier </param>
		/// <param name="renderComplete">Semaphore signalling that current frame's render frame is now complete. </param>
		void EndDraw (uint[] nextImage, IMgCommandBuffer prePresent, IMgSemaphore[] renderComplete);
	}
}

