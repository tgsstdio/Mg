namespace Magnesium.OpenGL
{
	public interface IGLSemaphore : IMgSemaphore
	{
		bool IsWaiting { get; }
		void Reset ();
		void BeginSync();
		bool IsReady ();

		long Duration { get; set; }
		int Factor { get; set; }
		uint TotalBlockingWaits { get; }
		uint TotalFailures { get; }
		uint BlockingRetries { get; set; }
		uint NonBlockingRetries { get; set;} 
	}
}

