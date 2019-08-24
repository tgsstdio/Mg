﻿using Magnesium.OpenGL.Internals;
using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLCmdQueue : IGLQueue
	{
		private IGLCmdStateRenderer mRenderer;
		private IGLSemaphoreEntrypoint mSignalModule;

		~GLCmdQueue()
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
        private readonly IGLBlitOperationEntrypoint mBlit;
        public GLCmdQueue (
            IGLCmdStateRenderer renderer,
            IGLBlitOperationEntrypoint blit,
            IGLSemaphoreEntrypoint generator,
            IGLCmdImageEntrypoint imageOps)
		{
            mRenderer = renderer;
            mBlit = blit;

			mSignalModule = generator;
			mImageOps = imageOps;
		}

		#region IMgQueue implementation
		private Dictionary<uint, GLQueueSubmission> mSubmissions = new Dictionary<uint, GLQueueSubmission>();
		private Dictionary<uint, GLQueueSubmitOrder> mOrders = new Dictionary<uint, GLQueueSubmitOrder>();

		MgResult CompleteAllPreviousSubmissions (IMgFence fence)
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
				return MgResult.SUCCESS;
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

		public MgResult QueueSubmit (MgSubmitInfo[] pSubmits, IMgFence fence)
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
					order.Fence = (IGLFence) fence;
					foreach (var sub in children)
					{
						order.Submissions.Add (sub.Key, sub.OrderFence);
					}
					// JUST LOOP AROUND
					mOrderKey = (mOrderKey >= uint.MaxValue) ? 0 : mOrderKey + 1;
					mOrders.Add (order.Key, order);
				}

				return MgResult.SUCCESS;
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
                            Render(buffer);
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

		public MgResult QueueWaitIdle ()
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
							var fence = order.Fence;
                            fence.Reset();
                            fence.BeginSync();
							mOrders.Remove (orderKey);
						}
					}
				}

			} while (!IsEmpty());

			return MgResult.SUCCESS;
		}

		public bool IsEmpty ()
		{
			return (mSubmissions.Keys.Count == 0 && mOrders.Keys.Count == 0);
		}

		public MgResult QueueBindSparse (MgBindSparseInfo[] pBindInfo, IMgFence fence)
		{
			throw new NotImplementedException ();
		}

		public MgResult QueuePresentKHR (MgPresentInfoKHR pPresentInfo)
		{
			// EARLY EXIT
			if (pPresentInfo == null)
			{
				return MgResult.SUCCESS;
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

			return MgResult.SUCCESS;
		}

        #endregion

        #region Render methods

        public void Render(IGLCommandBuffer buffer)
        {
            if (buffer.IsQueueReady)
            {
                var recording = GenerateRecording(buffer, mRenderer);

                foreach (var context in buffer.Record.Contexts)
                {
                    if (context.Category == GLCmdEncoderCategory.Compute)
                    {
                        recording.Compute.Encoder = new GLCmdComputeEncoder();
                    }
                    else if (context.Category == GLCmdEncoderCategory.Blit)
                    {
                        recording.Blit.Entrypoint = mBlit;
                    }

                    for (var i = context.First; i <= context.Last; ++i)
                    {
                        buffer.Record.Instructions[i].Perform(recording);
                    }

                    if (context.Category == GLCmdEncoderCategory.Compute)
                    {
                        recording.Compute.Encoder.EndEncoding();
                        recording.Compute.Encoder = null;
                    }
                    ////else if (context.Category == AmtEncoderCategory.Blit)
                    ////{
                    ////	recording.Blit.Encoder.EndEncoding();
                    ////	recording.Blit.Encoder = null;
                    ////}
                }

            }
        }

        static GLCmdCommandRecording GenerateRecording(IGLCommandBuffer buffer, IGLCmdStateRenderer renderer)
        {
            return new GLCmdCommandRecording
            {
                Compute = new GLCmdComputeRecording
                {
                    Grid = buffer.Record.ComputeGrid,
                },
                Graphics = new GLCmdGraphicsRecording
                {
                    StateRenderer = renderer,
                    Grid = buffer.Record.GraphicsGrid,
                },
                Blit = new GLCmdBlitRecording
                {
                    Grid = buffer.Record.BlitGrid,
                },
            };
        }

        public void GetQueueCheckpointDataNV(out MgCheckpointDataNV[] pCheckpointData)
        {
            throw new NotImplementedException();
        }

        public void QueueBeginDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo)
        {
            throw new NotImplementedException();
        }

        public void QueueInsertDebugUtilsLabelEXT(MgDebugUtilsLabelEXT labelInfo)
        {
            throw new NotImplementedException();
        }

        public void QueueEndDebugUtilsLabelEXT()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

