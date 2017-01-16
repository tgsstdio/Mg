using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class GLFullFence : IGLFence
    {
        public int Index { get; set; }
        public IntPtr ObjectPtr { get; set; }
        public float Timeout { get; set; }
        private ClientWaitSyncFlags mWaitOption;

        public bool IsSignalled
        {
            get;
            private set;        
        }

        public GLFullFence()
        {
            IsSignalled = true;
            ObjectPtr = IntPtr.Zero;
        }

        public void Reset()
        {
            IsSignalled = true;
            if (ObjectPtr != IntPtr.Zero)
            {
                GL.DeleteSync(ObjectPtr);
                ObjectPtr = IntPtr.Zero;
            }
        }

        private uint mNonblockCount;
        public void BeginSync()
        {
            ObjectPtr = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, 0);
            IsSignalled = false;
            mWaitOption = ClientWaitSyncFlags.SyncFlushCommandsBit;
        }

        private bool NonBlockingWait()
        {
            // only on the first time

            WaitSyncStatus result = GL.ClientWaitSync(ObjectPtr, mWaitOption, 0);
            if (result == WaitSyncStatus.WaitFailed)
            {
                throw new InvalidOperationException("GPU NonBlockingWait sync failed - surplus actions incomplete");
            }

            mWaitOption = ClientWaitSyncFlags.None;
            // HAS NOT COMPLETED
            return !(result == WaitSyncStatus.ConditionSatisfied || result == WaitSyncStatus.AlreadySignaled);
        }

        private bool BlockingWait(long timeInNanoSecs)
        {
            WaitSyncStatus status = GL.ClientWaitSync(ObjectPtr, mWaitOption, timeInNanoSecs);
            // BLOCKING WAITING 
            if (status == WaitSyncStatus.WaitFailed)
            {
                throw new InvalidOperationException("GPU BlockingWait sync failed - surplus actions completed");
            }
            // HAS NOT COMPLETED
            return (status == WaitSyncStatus.ConditionSatisfied || status == WaitSyncStatus.AlreadySignaled);
        }

        public bool IsReady(long timeInNanoSecs)
        {
            if (!IsSignalled)
            {
                bool needBlocking = true;
                if (IsFirstFenceCheck())
                {
                    needBlocking = NonBlockingWait();
                }

                if (needBlocking)
                {
                    IsSignalled = BlockingWait(timeInNanoSecs);
                }
                else
                {
                    IsSignalled = true;
                }
            }

            return IsSignalled;
        }

        private bool IsFirstFenceCheck()
        {
            return mWaitOption == ClientWaitSyncFlags.SyncFlushCommandsBit;
        }

        public void DestroyFence(IMgDevice device, IMgAllocationCallbacks allocator)
        {
            if (ObjectPtr != IntPtr.Zero)
            {
                GL.DeleteSync(ObjectPtr);
                ObjectPtr = IntPtr.Zero;
            }
        }
    }
}
