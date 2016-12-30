using OpenTK;
using System;
using System.Diagnostics;

//
// David Young 2016
// Code taken from the GameWindow.cs, INativeWindow.cs, NativeWindow.cs
// 
// The Open Toolkit Library License
//
// Copyright (c) 2006 - 2009 the Open Toolkit library.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//

namespace TextureDemo
{
    class GameWindow : IDisposable
    {
        const double MaxFrequency = 500.0; // Frequency cap for Update/RenderFrame events

        readonly Stopwatch watch = new Stopwatch();

        double target_update_period, target_render_period;

        private INativeWindow implementation;

        bool isExiting = false;
        bool events;

        /// <summary>
        /// System.Threading.Thread.CurrentThread.ManagedThreadId of the thread that created this <see cref="OpenTK.NativeWindow"/>.
        /// </summary>
        private int thread_id;

        //private bool events;

        double update_period;
        double render_period;
        double update_timestamp; // timestamp of last UpdateFrame event
        double render_timestamp; // timestamp of last RenderFrame event
        double update_epsilon; // quantization error for UpdateFrame events

        double render_time; // length of last RenderFrame event
        double update_time; // length of last UpdateFrame event

        bool is_running_slowly; // true, when UpdatePeriod cannot reach TargetUpdatePeriod

        public GameWindow(INativeWindow window)
        {
            implementation = window;

            this.thread_id = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        #region EnsureUndisposed

        /// <summary>
        /// Ensures that this NativeWindow has not been disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// If this NativeWindow has been disposed.
        /// </exception>
        protected void EnsureUndisposed()
        {
            if (mIsDisposed) throw new ObjectDisposedException(GetType().Name);
        }

        #endregion


        #region Visible

        /// <summary>
        /// Gets or sets a System.Boolean that indicates whether this NativeWindow is visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                EnsureUndisposed();
                return implementation.Visible;
            }
            set
            {
                EnsureUndisposed();
                implementation.Visible = value;
            }
        }

        #endregion

        #region TargetUpdatePeriod

        /// <summary>
        /// Gets or sets a double representing the target update period, in seconds.
        /// </summary>
        /// <remarks>
        /// <para>A value of 0.0 indicates that UpdateFrame events are generated at the maximum possible frequency (i.e. only limited by the hardware's capabilities).</para>
        /// <para>Values lower than 0.002 seconds (500Hz) are clamped to 0.0. Values higher than 1.0 seconds (1Hz) are clamped to 1.0.</para>
        /// </remarks>
        public double TargetUpdatePeriod
        {
            get
            {
                EnsureUndisposed();
                return target_update_period;
            }
            set
            {
                EnsureUndisposed();
                if (value <= 1 / MaxFrequency)
                {
                    target_update_period = 0.0;
                }
                else if (value <= 1.0)
                {
                    target_update_period = value;
                }
                else Debug.Print("Target update period clamped to 1.0 seconds.");
            }
        }

        #endregion

        #region TargetUpdateFrequency

        /// <summary>
        /// Gets or sets a double representing the target update frequency, in hertz.
        /// </summary>
        /// <remarks>
        /// <para>A value of 0.0 indicates that UpdateFrame events are generated at the maximum possible frequency (i.e. only limited by the hardware's capabilities).</para>
        /// <para>Values lower than 1.0Hz are clamped to 0.0. Values higher than 500.0Hz are clamped to 500.0Hz.</para>
        /// </remarks>
        public double TargetUpdateFrequency
        {
            get
            {
                EnsureUndisposed();
                if (TargetUpdatePeriod == 0.0)
                    return 0.0;
                return 1.0 / TargetUpdatePeriod;
            }
            set
            {
                EnsureUndisposed();
                if (value < 1.0)
                {
                    TargetUpdatePeriod = 0.0;
                }
                else if (value <= MaxFrequency)
                {
                    TargetUpdatePeriod = 1.0 / value;
                }
                else Debug.Print("Target render frequency clamped to {0}Hz.", MaxFrequency);
            }
        }

        #endregion

        /// <summary>
        /// Enters the game loop of the GameWindow updating and rendering at the specified frequency.
        /// </summary>
        /// <remarks>
        /// When overriding the default game loop you should call ProcessEvents()
        /// to ensure that your GameWindow responds to operating system events.
        /// <para>
        /// Once ProcessEvents() returns, it is time to call update and render the next frame.
        /// </para>
        /// </remarks>
        /// <param name="updates_per_second">The frequency of UpdateFrame events.</param>
        /// <param name="frames_per_second">The frequency of RenderFrame events.</param>
        public void Run(double updates_per_second, double frames_per_second)
        {
            EnsureUndisposed();

            try
            {
                if (updates_per_second < 0.0 || updates_per_second > 200.0)
                    throw new ArgumentOutOfRangeException("updates_per_second", updates_per_second,
                                                          "Parameter should be inside the range [0.0, 200.0]");
                if (frames_per_second < 0.0 || frames_per_second > 200.0)
                    throw new ArgumentOutOfRangeException("frames_per_second", frames_per_second,
                                                          "Parameter should be inside the range [0.0, 200.0]");

                if (updates_per_second != 0)
                {
                    TargetUpdateFrequency = updates_per_second;
                }
                if (frames_per_second != 0)
                {
                    TargetRenderFrequency = frames_per_second;
                }

                Visible = true;   // Make sure the GameWindow is visible.
                //OnLoadInternal(EventArgs.Empty);
                //OnResize(EventArgs.Empty);

                // On some platforms, ProcessEvents() does not return while the user is resizing or moving
                // the window. We can avoid this issue by raising UpdateFrame and RenderFrame events
                // whenever we encounter a size or move event.
                // Note: hack disabled. Threaded rendering provides a better solution to this issue.
                //Move += DispatchUpdateAndRenderFrame;
                //Resize += DispatchUpdateAndRenderFrame;

                Debug.Print("Entering main loop.");
                watch.Start();
                while (true)
                {
                    ProcessEvents(false);
                    if (Exists && !IsExiting)
                        DispatchUpdateAndRenderFrame(this, EventArgs.Empty);
                    else
                        return;
                }
            }
            finally
            {
                Move -= DispatchUpdateAndRenderFrame;
                Resize -= DispatchUpdateAndRenderFrame;

                if (Exists)
                {
                    // TODO: Should similar behaviour be retained, possibly on native window level?
                    //while (this.Exists)
                    //    ProcessEvents(false);
                }
            }
        }

        /// <summary>This member is not supported.</summary>
        /// <value>To be added.</value>
        /// <remarks>
        ///   <para>
        ///     Throws a <see cref="T:System.NotSupportedException" />.
        ///   </para>
        /// </remarks>
        public event EventHandler<EventArgs> Move = delegate { };

        /// <summary>
        ///   Occurs when the view's
        ///   <see cref="P:OpenTK.Platform.Android.AndroidGameView.Size" />
        ///   changes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public event EventHandler<EventArgs> Resize = delegate { };

        void DispatchUpdateAndRenderFrame(object sender, EventArgs e)
        {
            int is_running_slowly_retries = 4;
            double timestamp = watch.Elapsed.TotalSeconds;
            double elapsed = 0;

            elapsed = ClampElapsed(timestamp - update_timestamp);
            while (elapsed > 0 && elapsed + update_epsilon >= TargetUpdatePeriod)
            {
                RaiseUpdateFrame(elapsed, ref timestamp);

                // Calculate difference (positive or negative) between
                // actual elapsed time and target elapsed time. We must
                // compensate for this difference.
                update_epsilon += elapsed - TargetUpdatePeriod;

                // Prepare for next loop
                elapsed = ClampElapsed(timestamp - update_timestamp);

                if (TargetUpdatePeriod <= Double.Epsilon)
                {
                    // According to the TargetUpdatePeriod documentation,
                    // a TargetUpdatePeriod of zero means we will raise
                    // UpdateFrame events as fast as possible (one event
                    // per ProcessEvents() call)
                    break;
                }

                is_running_slowly = update_epsilon >= TargetUpdatePeriod;
                if (is_running_slowly && --is_running_slowly_retries == 0)
                {
                    // If UpdateFrame consistently takes longer than TargetUpdateFrame
                    // stop raising events to avoid hanging inside the UpdateFrame loop.
                    break;
                }
            }

            elapsed = ClampElapsed(timestamp - render_timestamp);
            if (elapsed > 0 && elapsed >= TargetRenderPeriod)
            {
                RaiseRenderFrame(elapsed, ref timestamp);
            }
        }

        void RaiseUpdateFrame(double elapsed, ref double timestamp)
        {
            // Raise UpdateFrame event
            OnUpdateFrameInternal(new FrameEventArgs(elapsed));

            // Update UpdatePeriod/UpdateFrequency properties
            update_period = elapsed;

            // Update UpdateTime property
            update_timestamp = timestamp;
            timestamp = watch.Elapsed.TotalSeconds;
            update_time = timestamp - update_timestamp;
        }

        /// <summary>
        /// Occurs when it is time to update a frame.
        /// </summary>
        public event EventHandler<FrameEventArgs> UpdateFrame = delegate { };

        #region OnUpdateFrame

        /// <summary>
        /// Called when the frame is updated.
        /// </summary>
        /// <param name="e">Contains information necessary for frame updating.</param>
        /// <remarks>
        /// Subscribe to the <see cref="UpdateFrame"/> event instead of overriding this method.
        /// </remarks>
        protected virtual void OnUpdateFrame(FrameEventArgs e)
        {
            UpdateFrame(this, e);
        }

        #endregion

        #region OnUpdateFrameInternal

        private void OnUpdateFrameInternal(FrameEventArgs e) { if (Exists && !isExiting) OnUpdateFrame(e); }

        #endregion

        public event EventHandler<FrameEventArgs> RenderFrame = delegate { };

        #region OnRenderFrame

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        /// <param name="e">Contains information necessary for frame rendering.</param>
        /// <remarks>
        /// Subscribe to the <see cref="RenderFrame"/> event instead of overriding this method.
        /// </remarks>
        protected virtual void OnRenderFrame(FrameEventArgs e)
        {
            RenderFrame(this, e);
        }

        #endregion

        #region OnRenderFrameInternal

        private void OnRenderFrameInternal(FrameEventArgs e) { if (Exists && !isExiting) OnRenderFrame(e); }

        #endregion

        void RaiseRenderFrame(double elapsed, ref double timestamp)
        {
            // Raise RenderFrame event
            OnRenderFrameInternal(new FrameEventArgs(elapsed));

            // Update RenderPeriod/UpdateFrequency properties
            render_period = elapsed;

            // Update RenderTime property
            render_timestamp = timestamp;
            timestamp = watch.Elapsed.TotalSeconds;
            render_time = timestamp - render_timestamp;
        }

        double ClampElapsed(double elapsed)
        {
            return MathHelper.Clamp(elapsed, 0.0, 1.0);
        }

        #region IsExiting

        /// <summary>
        /// Gets a value indicating whether the shutdown sequence has been initiated
        /// for this window, by calling GameWindow.Exit() or hitting the 'close' button.
        /// If this property is true, it is no longer safe to use any OpenTK.Input or
        /// OpenTK.Graphics.OpenGL functions or properties.
        /// </summary>
        public bool IsExiting
        {
            get
            {
                EnsureUndisposed();
                return isExiting;
            }
        }

        #endregion

        #region Exists

        /// <summary>
        /// Gets a value indicating whether a render window exists.
        /// </summary>
        public bool Exists
        {
            get
            {
                return mIsDisposed ? false : implementation.Exists; // TODO: Should disposed be ignored instead?
            }
        }

        #endregion

        #region ProcessEvents

        /// <summary>
        /// Processes operating system events until the NativeWindow becomes idle.
        /// </summary>
        /// <param name="retainEvents">If true, the state of underlying system event propagation will be preserved, otherwise event propagation will be enabled if it has not been already.</param>
        protected void ProcessEvents(bool retainEvents)
        {
            EnsureUndisposed();
            if (this.thread_id != System.Threading.Thread.CurrentThread.ManagedThreadId)
            {
                throw new InvalidOperationException("ProcessEvents must be called on the same thread that created the window.");
            }
            if (!retainEvents && !events) Events = true;
            implementation.ProcessEvents();
        }

        #endregion

        #region OnMove

        /// <summary>
        /// Called when the NativeWindow is moved.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected virtual void OnMove(EventArgs e)
        {
            Move(this, e);
        }

        #endregion

        #region OnMoveInternal

        private void OnMoveInternal(object sender, EventArgs e) { OnMove(e); }

        #endregion

        #region OnResize

        /// <summary>
        /// Called when the NativeWindow is resized.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected virtual void OnResize(EventArgs e)
        {
            Resize(this, e);
        }

        #endregion

        #region OnResizeInternal

        private void OnResizeInternal(object sender, EventArgs e) { OnResize(e); }

        #endregion

        #region Events

        private bool Events
        {
            set
            {
                if (value)
                {
                    if (events)
                    {
                        throw new InvalidOperationException("Event propagation is already enabled.");
                    }
                    implementation.Move += OnMoveInternal;
                    implementation.Resize += OnResizeInternal;
                    events = true;
                }
                else if (events)
                {
                    implementation.Move -= OnMoveInternal;
                    implementation.Resize -= OnResizeInternal;
                    events = false;
                }
                else
                {
                    throw new InvalidOperationException("Event propagation is already disabled.");
                }
            }
        }

        #endregion


        #region TargetRenderFrequency

        /// <summary>
        /// Gets or sets a double representing the target render frequency, in hertz.
        /// </summary>
        /// <remarks>
        /// <para>A value of 0.0 indicates that RenderFrame events are generated at the maximum possible frequency (i.e. only limited by the hardware's capabilities).</para>
        /// <para>Values lower than 1.0Hz are clamped to 0.0. Values higher than 500.0Hz are clamped to 200.0Hz.</para>
        /// </remarks>
        public double TargetRenderFrequency
        {
            get
            {
                EnsureUndisposed();
                if (TargetRenderPeriod == 0.0)
                    return 0.0;
                return 1.0 / TargetRenderPeriod;
            }
            set
            {
                EnsureUndisposed();
                if (value < 1.0)
                {
                    TargetRenderPeriod = 0.0;
                }
                else if (value <= MaxFrequency)
                {
                    TargetRenderPeriod = 1.0 / value;
                }
                else Debug.Print("Target render frequency clamped to {0}Hz.", MaxFrequency);
            }
        }

        #endregion

        #region TargetRenderPeriod

        /// <summary>
        /// Gets or sets a double representing the target render period, in seconds.
        /// </summary>
        /// <remarks>
        /// <para>A value of 0.0 indicates that RenderFrame events are generated at the maximum possible frequency (i.e. only limited by the hardware's capabilities).</para>
        /// <para>Values lower than 0.002 seconds (500Hz) are clamped to 0.0. Values higher than 1.0 seconds (1Hz) are clamped to 1.0.</para>
        /// </remarks>
        public double TargetRenderPeriod
        {
            get
            {
                EnsureUndisposed();
                return target_render_period;
            }
            set
            {
                EnsureUndisposed();
                if (value <= 1 / MaxFrequency)
                {
                    target_render_period = 0.0;
                }
                else if (value <= 1.0)
                {
                    target_render_period = value;
                }
                else Debug.Print("Target render period clamped to 1.0 seconds.");
            }
        }

        #endregion

        #region IDisposable Support
        private bool mIsDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            mIsDisposed = true;
            
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
