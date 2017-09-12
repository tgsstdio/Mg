using System;
using System.IO;

namespace TriangleDemo.MetalMac
{
	class MetalTriangleDemoShaderPath : ITriangleDemoShaderPath
	{
		public Stream OpenFragmentShader()
		{
			return File.OpenRead("triangleFrag.metal");
		}

		public Stream OpenVertexShader()
		{
			return File.OpenRead("triangleVert.metal");
		}
	}
}