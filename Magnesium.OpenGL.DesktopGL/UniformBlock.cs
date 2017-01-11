using System;

namespace BirdNest.Core
{
	public class UniformBlock
	{
		public Type BlockType { get; set; }
		public Type ReferenceType { get; set; }
		public int BlockIndex { get; set;}
		public int UniformLocation {get;set;}
		public bool HasIndex {get;set;}
		public int IndexLocation {get;set;}
		public int IndexValue {get;set;}
	}
}

