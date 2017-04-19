using Magnesium.OpenGL.Internals;
using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
    [TestFixture]
	public class CollatorTest
	{
		[Test]
		public void TestCase()
		{
			IGLUniformBlockNameParser tokenizer = new DefaultGLUniformBlockNameParser();
			var token0 = tokenizer.Parse("UBO[0]");
			token0.BindingIndex = 10;
			var collator = new GLUniformBlockGroupCollator();
			collator.Add(token0);
			var token1 = tokenizer.Parse("UBO[1]");
			token1.BindingIndex = 10;
			collator.Add(token1);

			var groups = collator.Collate();
			Assert.AreEqual(1, groups.Values.Count);
			var firstGroup = groups[10];
			Assert.AreEqual("UBO", firstGroup.Prefix);
			Assert.AreEqual(2, firstGroup.Count);
		}

		[Test]
		public void TestCase2()
		{
			var token0 = new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 0,
				Y = 0,
				Z = 0,
			};
			var collator = new GLUniformBlockGroupCollator();
			collator.Add(token0);
			var token1 = new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 15,
				X = 1,
				Y = 0,
				Z = 0,
			};
			collator.Add(token1);

			var groups = collator.Collate();
			Assert.AreEqual(1, groups.Values.Count);
			var firstGroup = groups[10];
			Assert.AreEqual("UBO", firstGroup.Prefix);
			Assert.AreEqual(2, firstGroup.Count);
			Assert.AreEqual(token0.BindingIndex, firstGroup.FirstBinding);
		}

		[Test()]
		public void TestCase3()
		{
			var token0 = new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 0,
				Y = 0,
				Z = 0,
			};
			var collator = new GLUniformBlockGroupCollator();
			collator.Add(token0);
			var token1 = new GLUniformBlockInfo
			{
				Prefix = "ubo",
				BindingIndex = 15,
				X = 1,
				Y = 0,
				Z = 0,
			};
			collator.Add(token1);

			var groups = collator.Collate();
			Assert.AreEqual(2, groups.Values.Count);

			// SHOULD BE SORTED BY BINDING INDEX
			{
				var g = groups[10];
				Assert.AreEqual("UBO", g.Prefix);
				Assert.AreEqual(1, g.Count);
				Assert.AreEqual(10, g.FirstBinding);
			}

			{
				var g = groups[15];
				Assert.AreEqual("ubo", g.Prefix);
				Assert.AreEqual(1, g.Count);
				Assert.AreEqual(15, g.FirstBinding);
			}
		}

		[Test()]
		public void GroupsSorted1()
		{
			// SHOULD BE SORTED BY BINDING INDEX OF FIRST ELEMENT
			var token0 = new GLUniformBlockInfo
			{
				Prefix = "C",
				BindingIndex = 10,
				X = 0,
				Y = 0,
				Z = 0,
			};
			var collator = new GLUniformBlockGroupCollator();
			collator.Add(token0);
			var token1 = new GLUniformBlockInfo
			{
				Prefix = "A",
				BindingIndex = 12,
				X = 1,
				Y = 0,
				Z = 0,
			};
			collator.Add(token1);

			var token2 = new GLUniformBlockInfo
			{
				Prefix = "B",
				BindingIndex = 11,
				X = 1,
				Y = 0,
				Z = 0,
			};
			collator.Add(token2);

			var groups = collator.Collate();
			Assert.AreEqual(3, groups.Values.Count);

			// SHOULD BE SORTED BY BINDING INDEX
			{
				var group = groups[10];
				Assert.AreEqual("C", group.Prefix);
				Assert.AreEqual(1, group.Count);
				Assert.AreEqual(token0.BindingIndex, group.FirstBinding);
			}

			{
				var group = groups[11];
				Assert.AreEqual("B", group.Prefix);
				Assert.AreEqual(1, group.Count);
				Assert.AreEqual(token2.BindingIndex, group.FirstBinding);
			}

			{
				var group = groups[12];
				Assert.AreEqual("A", group.Prefix);
				Assert.AreEqual(1, group.Count);
				Assert.AreEqual(token1.BindingIndex, group.FirstBinding);
			}
		}

		[Test()]
		public void RowOffset0()
		{
			var token0 = new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 0,
				Y = 0,
				Z = 0,
			};
			var collator = new GLUniformBlockGroupCollator();
			collator.Add(token0);
			var token1 = new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 1,
				Y = 0,
				Z = 0,
			};
			collator.Add(token1);

			var groups = collator.Collate();
			Assert.AreEqual(1, groups.Values.Count);
			{
				var firstGroup = groups[10];
				Assert.AreEqual("UBO", firstGroup.Prefix);
				Assert.AreEqual(2, firstGroup.Count);
				Assert.AreEqual(10, firstGroup.FirstBinding, nameof(firstGroup.FirstBinding));
				Assert.AreEqual(2, firstGroup.ArrayStride, nameof(firstGroup.ArrayStride));
				Assert.AreEqual(1, firstGroup.HighestRow, nameof(firstGroup.HighestRow));
				Assert.AreEqual(2, firstGroup.MatrixStride, nameof(firstGroup.MatrixStride));
				Assert.AreEqual(1, firstGroup.HighestLayer, nameof(firstGroup.HighestLayer));
				Assert.AreEqual(2, firstGroup.CubeStride, nameof(firstGroup.CubeStride));
			}
		}

		[Test()]
		public void RowOffset1()
		{
			var token0 = new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 0,
				Y = 0,
				Z = 0,
			};
			var collator = new GLUniformBlockGroupCollator();
			collator.Add(token0);
			var token1 = new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 2,
				Y = 2,
				Z = 0,
			};
			collator.Add(token1);

			var groups = collator.Collate();
			Assert.AreEqual(1, groups.Values.Count);
			{
				var firstGroup = groups[10];
				Assert.AreEqual("UBO", firstGroup.Prefix);
				Assert.AreEqual(2, firstGroup.Count);
				Assert.AreEqual(10, firstGroup.FirstBinding, nameof(firstGroup.FirstBinding));
				Assert.AreEqual(3, firstGroup.ArrayStride, nameof(firstGroup.ArrayStride));
				Assert.AreEqual(3, firstGroup.HighestRow, nameof(firstGroup.HighestRow));
				Assert.AreEqual(9, firstGroup.MatrixStride,nameof(firstGroup.MatrixStride));
				Assert.AreEqual(1, firstGroup.HighestLayer, nameof(firstGroup.HighestLayer));
				Assert.AreEqual(9, firstGroup.CubeStride, nameof(firstGroup.CubeStride));
			}
		}


		[Test()]
		public void RowOffset2()
		{
			var collator = new GLUniformBlockGroupCollator();
			collator.Add(new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 4,
				Y = 0,
				Z = 0,
			});

			collator.Add(new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 0,
				Y = 0,
				Z = 0,
			});

			collator.Add(new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 8,
				Y = 0,
				Z = 0,
			});

			var groups = collator.Collate();
			Assert.AreEqual(1, groups.Values.Count);
			{
				var firstGroup = groups[10];
				Assert.AreEqual("UBO", firstGroup.Prefix);
				Assert.AreEqual(3, firstGroup.Count);
				Assert.AreEqual(10, firstGroup.FirstBinding, nameof(firstGroup.FirstBinding));
				Assert.AreEqual(9, firstGroup.ArrayStride, nameof(firstGroup.ArrayStride));
				Assert.AreEqual(1, firstGroup.HighestRow, nameof(firstGroup.HighestRow));
				Assert.AreEqual(9, firstGroup.MatrixStride, nameof(firstGroup.MatrixStride));
				Assert.AreEqual(1, firstGroup.HighestLayer, nameof(firstGroup.HighestLayer));
				Assert.AreEqual(9, firstGroup.CubeStride, nameof(firstGroup.CubeStride));
			}
		}
		[Test()]
		public void RowOffset3()
		{
			// TODO : row major or column major
			var collator = new GLUniformBlockGroupCollator();
			collator.Add(new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 4,
				Y = 0,
				Z = 0,
			});
	
			collator.Add(new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 2,
				Y = 3,
				Z = 0,
			});

			collator.Add(new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 2,
				Y = 2,
				Z = 0,
			});

			var groups = collator.Collate();
			Assert.AreEqual(1, groups.Values.Count);
			{
				var firstGroup = groups[10];
				Assert.AreEqual("UBO", firstGroup.Prefix);
				Assert.AreEqual(3, firstGroup.Count);
				Assert.AreEqual(10, firstGroup.FirstBinding, nameof(firstGroup.FirstBinding));
				Assert.AreEqual(5, firstGroup.ArrayStride, nameof(firstGroup.ArrayStride));
				Assert.AreEqual(4, firstGroup.HighestRow, nameof(firstGroup.HighestRow));
				Assert.AreEqual(20, firstGroup.MatrixStride, nameof(firstGroup.MatrixStride));
				Assert.AreEqual(1, firstGroup.HighestLayer, nameof(firstGroup.HighestLayer));
				Assert.AreEqual(20, firstGroup.CubeStride, nameof(firstGroup.CubeStride));
			}
		}

		[Test]
		public void RowOffset4()
		{
			var collator = new GLUniformBlockGroupCollator();
			collator.Add(new GLUniformBlockInfo
			{
				Prefix = "UBO",
				BindingIndex = 10,
				X = 7,
				Y = 5,
				Z = 3,
			});

			var groups = collator.Collate();
			Assert.AreEqual(1, groups.Values.Count);
			{
				var firstGroup = groups[10];
				Assert.AreEqual("UBO", firstGroup.Prefix);
				Assert.AreEqual(1, firstGroup.Count);
				Assert.AreEqual(10, firstGroup.FirstBinding, nameof(firstGroup.FirstBinding));
				Assert.AreEqual(8, firstGroup.ArrayStride, nameof(firstGroup.ArrayStride));
				Assert.AreEqual(6, firstGroup.HighestRow, nameof(firstGroup.HighestRow));
                Assert.AreEqual(48, firstGroup.MatrixStride, nameof(firstGroup.MatrixStride));
				Assert.AreEqual(4, firstGroup.HighestLayer, nameof(firstGroup.HighestLayer));
				Assert.AreEqual(192, firstGroup.CubeStride, nameof(firstGroup.CubeStride));
			}
		}
	}
}
