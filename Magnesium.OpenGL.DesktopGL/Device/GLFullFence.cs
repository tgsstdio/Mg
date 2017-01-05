using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class GLFullFence : IGLFence
    {
        public int Index { get; set; }
        public IntPtr ObjectPtr { get; set; }
        public float Timeout { get; set; }

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
        }

        private bool NonBlockingWait()
        {
            // only on the first time
            ClientWaitSyncFlags waitOption = ClientWaitSyncFlags.SyncFlushCommandsBit;

            WaitSyncStatus result = GL.ClientWaitSync(ObjectPtr, waitOption, 0);
            if (result == WaitSyncStatus.WaitFailed)
            {
                throw new InvalidOperationException("GPU NonBlockingWait sync failed - surplus actions incomplete");
            }
            // HAS NOT COMPLETED
            return !(result == WaitSyncStatus.ConditionSatisfied || result == WaitSyncStatus.AlreadySignaled);
        }

        private bool BlockingWait(long timeInNanoSecs)
        {
            WaitSyncStatus status = GL.ClientWaitSync(ObjectPtr, ClientWaitSyncFlags.None, timeInNanoSecs);
            // BLOCKING WAITING 
            if (status == WaitSyncStatus.WaitFailed)
            {
                throw new InvalidOperationException("GPU BlockingWait sync failed - surplus actions completed");
            }
            // HAS NOT COMPLETED
            return !(status == WaitSyncStatus.ConditionSatisfied || status == WaitSyncStatus.AlreadySignaled);
        }

        public bool IsReady(long timeInNanoSecs)
        {
            if (!IsSignalled)
            {
                bool needBlocking = NonBlockingWait();
                if (needBlocking)
                {
                    IsSignalled = !needBlocking && BlockingWait(timeInNanoSecs);
                }
            }

            return IsSignalled;
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
