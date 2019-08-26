// This code was written for the OpenTK library and has been released
// to the Public Domain.
// It is provided "as is" without express or implied warranty of any kind.

using Magnesium;
using Magnesium.OpenGL;
using Magnesium.OpenGL.Internals;
using Magnesium.Toolkit;

namespace Examples.Tutorial
{
    internal class MockGraphicsConfiguration : IMgGraphicsConfiguration
    {
        private GLDevice mDevice;

        public MockGraphicsConfiguration(DefaultGLDeviceEntrypoint deviceEntrypoint)
        {
            mDevice = new GLDevice(null, deviceEntrypoint, null);
        }

        public IMgDevice Device => mDevice;

        public IMgThreadPartition Partition => throw new System.NotImplementedException();

        public IMgQueue Queue => throw new System.NotImplementedException();

        public MgPhysicalDeviceMemoryProperties MemoryProperties => throw new System.NotImplementedException();

        public void Dispose()
        {
       
        }

        public void Initialize(uint width, uint height)
        {
           
        }
    }
}