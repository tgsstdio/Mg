using System;
using System.IO;

namespace Magnesium
{
    public class MgShaderModuleCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UIntPtr CodeSize { get; set;}
		public Stream Code { get; set; }
	}
}

