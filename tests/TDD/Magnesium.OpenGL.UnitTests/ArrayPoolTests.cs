using NUnit.Framework;
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.UnitTests
{
    [TestFixture]
	public class ArrayPoolTests
	{
		[TestCase]
		public void ConstructorTest0()
		{
			const uint NO_OF_ITEMS = 6;
			var items = new MockGLStaticBufferResource[0];
			var poolResource = new GLPoolResource<MockGLStaticBufferResource>(NO_OF_ITEMS, items);

			Assert.AreSame(items, poolResource.Items);
			Assert.AreEqual(NO_OF_ITEMS, poolResource.Count);

			var head = poolResource.Head;
			Assert.IsNotNull(head);
			Assert.AreEqual(NO_OF_ITEMS, head.Count);
			Assert.AreEqual(0, head.First);
			Assert.AreEqual(5, head.Last);
			Assert.IsNull(head.Next);
		}

		[TestCase]
		public void ConstructorTest1()
		{
			const uint NO_OF_ITEMS = 0;
			var res = new GLPoolResource<MockGLStaticBufferResource>(NO_OF_ITEMS, null);
            Assert.AreEqual(NO_OF_ITEMS, res.Count);
            Assert.IsNull(res.Items);
		}
	}
}
