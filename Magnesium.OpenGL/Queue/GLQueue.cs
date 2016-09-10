using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLQueue : IGLQueue
	{
		private IGLQueueRenderer mRenderer;
		private IGLSemaphoreEntrypoint mSignalModule;

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

		private IGLCmdImageEntrypoint mImageOps;

		public GLQueue (IGLQueueRenderer renderer, IGLSemaphoreEntrypoint generator, IGLCmdImageEntrypoint imageOps)
		{
			mRenderer = renderer;
			mSignalModule = generator;
			mImageOps = imageOps;
		}

		#region IMgQueue implementation
		private Dictionary<uint, GLQueueSubmission> mSubmissions = new Dictionary<uint, GLQueueSubmission>();
		private Dictionary<uint, GLQueueSubmitOrder> mOrders = new Dictionary<uint, GLQueueSubmitOrder>();

		Result CompleteAllPreviousSubmissions (IMgFence fence)
		{
			var internalFence = fence as IGLQueueFence;
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

		private uint mSubmissionKey = 0;
		private uint mOrderKey = 0;

		GLQueueSubmission EnqueueSubmission (MgSubmitInfo sub)
		{
			var submit = new GLQueueSubmission (mSubmissionKey, sub);
			// JUST LOOP AROUND
			mSubmissionKey = (mSubmissionKey >= uint.MaxValue) ? 0 : mSubmissionKey + 1;
			mSubmissions.Add (submit.Key, submit);
			return submit;
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
					order.Submissions = new Dictionary<uint, IGLSemaphore> ();
					order.Fence = fence as IGLQueueFence;
					foreach (var sub in children)
					{
						order.Submissions.Add (sub.Key, sub.OrderFence);
					}
					// JUST LOOP AROUND
					mOrderKey = (mOrderKey >= uint.MaxValue) ? 0 : mOrderKey + 1;
					mOrders.Add (order.Key, order);
				}

				return Result.SUCCESS;
			}
		}

		void PerformRequests (uint key)
		{
			GLQueueSubmission request;
			if (mSubmissions.TryGetValue (key, out request))
			{
				int requirements = request.Waits.Length;
				int checks = 0;
				foreach (var signal in request.Waits)
				{
					if (signal.IsReady ())
					{
						++checks;
					}
				}
				// render
				if (checks >= requirements)
				{
					if (request.CommandBuffers != null)
					{
						foreach (var buffer in request.CommandBuffers)
						{
							mImageOps.PerformOperation (buffer.ImageInstructions);

							// TRY TO FIGURE OUT HOW TO STOP CMDBUF EXECUTION WITHOUT CHANGING 
							if (buffer.IsQueueReady)
							{
								mRenderer.Render (new []{buffer.InstructionSet});
							}
						}
					}

					foreach (var signal in request.Signals)
					{
						signal.Reset ();
						signal.BeginSync ();
					}
					if (request.OrderFence != null)
					{
						request.OrderFence.Reset ();
						request.OrderFence.BeginSync ();
					}
					mSubmissions.Remove (key);
				}
			}
		}

		public Result QueueWaitIdle ()
		{
			do
			{
				var requestKeys = new uint[mSubmissions.Keys.Count];
				mSubmissions.Keys.CopyTo(requestKeys, 0);

				foreach(var key in requestKeys)
				{
					PerformRequests (key);
				}


				var orderKeys = new uint[mOrders.Keys.Count];
				mOrders.Keys.CopyTo(orderKeys, 0);
				foreach (var orderKey in orderKeys)
				{
					GLQueueSubmitOrder order;
					if (mOrders.TryGetValue(orderKey, out order))
					{
						var submissionKeys = new uint[order.Submissions.Keys.Count];
						order.Submissions.Keys.CopyTo(submissionKeys, 0);

						foreach (uint key in submissionKeys)
						{
							IGLSemaphore signal;
							if (order.Submissions.TryGetValue (key, out signal))
							{
								if (signal.IsReady ())
								{
									signal.Reset ();
									order.Submissions.Remove (key);
								}
							}
						}

						if (order.Submissions.Count <= 0)
						{
							order.Fence.Signal ();
							mOrders.Remove (orderKey);
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
				var sc = image.Swapchain as IGLSwapchainKHR;
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

