using System;

namespace CommandGen.UnitTests
{
    internal class MethodBodyNullCheck
    {
        public object Source { get; set; }
        public string Name { get; internal set; }

        internal double GetImplementation()
        {
            throw new NotImplementedException();
        }
    }
}