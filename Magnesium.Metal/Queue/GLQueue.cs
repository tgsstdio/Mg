using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Metal;

namespace Magnesium.Metal
{
	public class GLQueue : IMgQueue
	{
		private IAmtQueueRenderer mRenderer;

		~GLQueue()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private bool mIsDisposed = false;
		protected virtual void Dispose(bool dispose)
		{
			if (mIsDisposed)
				return;			

			foreach (var order in mOrders.Values)
			{
				foreach (var submission in order.Submissions.Values)
				{
					// RESET ALL FENCES CURRENTLY ATTACHED
					submission.Reset ();
				}
			}

			mIsDisposed = true;
		}

		private readonly IAmtSemaphoreEntrypoint mSignalModule;

		public GLQueue (IAmtQueueRenderer renderer, IAmtSemaphoreEntrypoint entrypoint)
		{
			mRenderer = renderer;
			mSignalModule = entrypoint;
		}

		#region IMgQueue implementation
		private ConcurrentDictionary<long, GLQueueSubmission> mSubmissions = new ConcurrentDictionary<long, GLQueueSubmission>();
		private ConcurrentDictionary<long, GLQueueSubmitOrder> mOrders = new ConcurrentDictionary<long, GLQueueSubmitOrder>();

		Result CompleteAllPreviousSubmissions (IMgFence fence)
		{
			var internalFence = fence as IAmtQueueFence;
			if (internalFence != null)
			{
				var result = QueueWaitIdle ();
				internalFence.Signal ();
				return result;
			}
			else
			{
				return Result.SUCCESS;
			}
		}

		private long mSubmissionKey = 0;
		private long mOrderKey = 0;

		GLQueueSubmission EnqueueSubmission (MgSubmitInfo sub)
		{
			var submit = new GLQueueSubmission (mSubmissionKey, sub);
			// JUST LOOP AROUND
			Interlocked.CompareExchange(ref mSubmissionKey, 0, long.MaxValue);
			Interlocked.Increment(ref mSubmissionKey);
			if (mSubmissions.TryAdd(submit.Key, submit))
			{
				return submit;
			}
			else
				return null;
		}

		public Result QueueSubmit (MgSubmitInfo[] pSubmits, IMgFence fence)
		{
			if (pSubmits == null)
			{				
				return CompleteAllPreviousSubmissions (fence);
			} 
			else
			{
				var children = new List<GLQueueSubmission> ();
	
				foreach (var sub in pSubmits)
				{					
					var submit = EnqueueSubmission (sub);
					if (fence != null)
					{
						submit.OrderFence = mSignalModule.CreateSemaphore ();
					}
					children.Add (submit);
				}

				if (fence != null)
				{
					var order = new GLQueueSubmitOrder ();
					order.Key = mOrderKey;
					order.Submissions = new ConcurrentDictionary<long, AmtSemaphore> ();
					order.Fence = fence as IAmtQueueFence;
					foreach (var sub in children)
					{
						order.Submissions.TryAdd (sub.Key, sub.OrderFence);
					}
					// JUST LOOP AROUND
					Interlocked.CompareExchange(ref mOrderKey, 0, long.MaxValue);
					Interlocked.Increment(ref mOrderKey);

					mOrders.TryAdd (order.Key, order);
				}

				return Result.SUCCESS;
			}
		}

		void PerformRequests (long key)
		{
			GLQueueSubmission request;
			if (mSubmissions.TryGetValue (key, out request))
			{
				int requirements = request.Waits.Length;
				int checks = 0;
				foreach (var signal in request.Waits)
				{
					if (signal.IsSignalled)
					{
						++checks;
					}
				}
				// render
				if (checks >= requirements)
				{
					mRenderer.Render(request);

					GLQueueSubmission removedItem;
					mSubmissions.TryRemove (key, out removedItem);
				}
			}
		}

		public Result QueueWaitIdle ()
		{
			do
			{
				var requestKeys = new long[mSubmissions.Keys.Count];
				mSubmissions.Keys.CopyTo(requestKeys, 0);

				foreach(var key in requestKeys)
				{
					PerformRequests (key);
				}


				var orderKeys = new long[mOrders.Keys.Count];
				mOrders.Keys.CopyTo(orderKeys, 0);
				foreach (var orderKey in orderKeys)
				{
					GLQueueSubmitOrder order;
					if (mOrders.TryGetValue(orderKey, out order))
					{
						var submissionKeys = new long[order.Submissions.Keys.Count];
						order.Submissions.Keys.CopyTo(submissionKeys, 0);

						foreach (uint key in submissionKeys)
						{
							AmtSemaphore signal;
							if (order.Submissions.TryGetValue (key, out signal))
							{
								if (signal.IsSignalled)
								{
									AmtSemaphore removedSemaphore;
									order.Submissions.TryRemove(key, out removedSemaphore);
								}
							}
						}

						if (order.Submissions.Count <= 0)
						{
							order.Fence.Signal();
							GLQueueSubmitOrder removedItem;
							mOrders.TryRemove (orderKey, out removedItem);
						}
					}
				}

			} while (!IsEmpty());

			return Result.SUCCESS;
		}

		public bool IsEmpty ()
		{
			return (mSubmissions.Keys.Count == 0 && mOrders.Keys.Count == 0);
		}

		public Result QueueBindSparse (MgBindSparseInfo[] pBindInfo, IMgFence fence)
		{
			throw new NotImplementedException ();
		}

		public Result QueuePresentKHR (MgPresentInfoKHR pPresentInfo)
		{
			throw new NotImplementedException();

			// EARLY EXIT
			if (pPresentInfo == null)
			{
				return Result.SUCCESS;
			}

			var signalInfos = new List<MgSubmitInfoWaitSemaphoreInfo> ();
			if (pPresentInfo.WaitSemaphores != null)
			{
				foreach (var signal in pPresentInfo.WaitSemaphores)
				{
					if (signal != null)
					{
						signalInfos.Add (new MgSubmitInfoWaitSemaphoreInfo {
							WaitDstStageMask = 0,
							WaitSemaphore = signal,
						});
					}
				}
			}

			var sub = new MgSubmitInfo {
				WaitSemaphores = signalInfos.ToArray(),
			};
			EnqueueSubmission (sub);

			foreach (var image in pPresentInfo.Images)
			{
				var sc = image.Swapchain as IAmtSwapchainKHR;
				if (sc != null)
				{
					sc.SwapBuffers ();
				}
			}

			return Result.SUCCESS;
		}

		#endregion
	}
}

