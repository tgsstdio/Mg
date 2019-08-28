using System;

namespace Magnesium
{
    public class MgColorFormatNotSupportedException : Exception
    {
        public MgColorFormatNotSupportedException() : base("Color format not supported.")
        {

        }
    }
}