using System;
using NUnit.Framework;
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.UnitTests
{
    [TestFixture]
	public class AllocatePoolResourceTests
	{
		const int NO_OF_ITEMS = 3;
		internal GLPoolResource<MockGLStaticBufferResource> PoolResource { get; private set; }

		[SetUp]
		public void Setup()
		{
			PoolResource = new GLPoolResource<MockGLStaticBufferResource>(NO_OF_ITEMS, null);
		}

		[TearDown]
		public void Cleanup()
		{
			PoolResource = null;
		}

		[TestCase]
		public void FailedAllocation_0()
		{
			GLPoolResourceTicket allocation;
			var result = PoolResource.Allocate(NO_OF_ITEMS + 1, out allocation);
			Assert.IsFalse(result);
			Assert.IsNull(allocation);
			Assert.IsNotNull(PoolResource.Head);
		}

        [TestCase]
        public void FailedAllocation_1()
        {
            GLPoolResourceTicket allocation;
            Assert.Throws<ArgumentOutOfRangeException>(() => { PoolResource.Allocate(0, out allocation); } ); 
		}

		[TestCase]
		public void AllItems()
		{
			GLPoolResourceTicket allocation;
			var result = PoolResource.Allocate(NO_OF_ITEMS, out allocation);
			Assert.IsTrue(result);
			Assert.IsNotNull(allocation);
			Assert.IsNull(PoolResource.Head);
		}

		[TestCase]
		public void LessThan_0()
		{
			const uint REQUESTED = NO_OF_ITEMS - 1;
			GLPoolResourceTicket allocation;
			var result = PoolResource.Allocate(REQUESTED, out allocation);
			Assert.IsTrue(result);
			Assert.IsNotNull(allocation);

			Assert.AreEqual(REQUESTED, allocation.Count);
			Assert.AreEqual(0, allocation.First);
			Assert.AreEqual(1, allocation.Last);

			var head = PoolResource.Head;
			Assert.IsNotNull(head);
			Assert.AreEqual(2, head.First);
			Assert.AreEqual(2, head.Last);
			Assert.AreEqual(1, head.Count);
		}

		[TestCase]
		public void LessThan_1()
		{
			const uint REQUESTED_0 = 2;

			GLPoolResourceTicket allocation_0;
			var result_0 = PoolResource.Allocate(REQUESTED_0, out allocation_0);
			Assert.IsTrue(result_0);
			Assert.IsNotNull(allocation_0);

			Assert.AreEqual(REQUESTED_0, allocation_0.Count);
			Assert.AreEqual(0, allocation_0.First);
			Assert.AreEqual(1, allocation_0.Last);

			var head = PoolResource.Head;
			Assert.IsNotNull(head);
			Assert.AreEqual(2, head.First);
			Assert.AreEqual(2, head.Last);
			Assert.AreEqual(1, head.Count);

			const uint REQUESTED_1 = 1;
			GLPoolResourceTicket allocation_1;
			var result_1 = PoolResource.Allocate(REQUESTED_1, out allocation_1);
			Assert.IsTrue(result_1);
			Assert.IsNotNull(allocation_1);

			head = PoolResource.Head;
			Assert.IsNull(head);

			Assert.AreEqual(REQUESTED_1, allocation_1.Count);
			Assert.AreEqual(2, allocation_1.First);
			Assert.AreEqual(2, allocation_1.Last);
		}

		[TestCase]
		public void LessThan_2()
		{
			const uint REQUESTED_0 = 1;

			GLPoolResourceTicket allocation_0;
			var result_0 = PoolResource.Allocate(REQUESTED_0, out allocation_0);
			Assert.IsTrue(result_0);
			Assert.IsNotNull(allocation_0);

			Assert.AreEqual(REQUESTED_0, allocation_0.Count);
			Assert.AreEqual(0, allocation_0.First);
			Assert.AreEqual(0, allocation_0.Last);

			var head = PoolResource.Head;
			Assert.IsNotNull(head);
			Assert.AreEqual(1, head.First);
			Assert.AreEqual(2, head.Last);
			Assert.AreEqual(2, head.Count);

			const uint REQUESTED_1 = 2;
			GLPoolResourceTicket allocation_1;
			var result_1 = PoolResource.Allocate(REQUESTED_1, out allocation_1);
			Assert.IsTrue(result_1);
			Assert.IsNotNull(allocation_1);

			head = PoolResource.Head;
			Assert.IsNull(head);

			Assert.AreEqual(REQUESTED_1, allocation_1.Count);
			Assert.AreEqual(1, allocation_1.First);
			Assert.AreEqual(2, allocation_1.Last);
		}

		[TestCase]
		public void LessThan_3()
		{
			const uint REQUESTED_0 = 1;

			GLPoolResourceTicket allocation_0;
			var result_0 = PoolResource.Allocate(REQUESTED_0, out allocation_0);
			Assert.IsTrue(result_0);
			Assert.IsNotNull(allocation_0);

			Assert.AreEqual(REQUESTED_0, allocation_0.Count);
			Assert.AreEqual(0, allocation_0.First);
			Assert.AreEqual(0, allocation_0.Last);

			var head = PoolResource.Head;
			Assert.IsNotNull(head);
			Assert.AreEqual(1, head.First);
			Assert.AreEqual(2, head.Last);
			Assert.AreEqual(2, head.Count);

			const uint REQUESTED_1 = 3;
			GLPoolResourceTicket allocation_1;
			var result_1 = PoolResource.Allocate(REQUESTED_1, out allocation_1);
			Assert.IsFalse(result_1);
			Assert.IsNull(allocation_1);

			head = PoolResource.Head;
			Assert.IsNotNull(head);
			Assert.AreEqual(1, head.First);
			Assert.AreEqual(2, head.Last);
			Assert.AreEqual(2, head.Count);
		}
	}
}
