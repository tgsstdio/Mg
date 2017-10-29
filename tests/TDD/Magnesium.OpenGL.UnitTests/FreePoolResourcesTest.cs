using NUnit.Framework;
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.UnitTests
{
    [TestFixture]
	public class FreePoolResourcesTest
	{
		const int NO_OF_ITEMS = 6;
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

		//[Test]
		//public void DifferentParent()
		//{
		//	GLPoolResourceInfo ticket;
		//	var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket);

		//	Assert.IsTrue(initialCheck);
		//	Assert.IsNull(PoolResource.Head);

		//	var parent = new GLPoolResource<MockGLStaticBufferResource>(1, null);
		//	var actual = PoolResource.Free(parent, ticket);
		//	Assert.IsFalse(actual);
		//	Assert.IsNull(PoolResource.Head);
		//}


		[Test]
		public void NewHead()
		{
			GLPoolResourceTicket ticket;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			var parent = PoolResource;
			var actual = PoolResource.Free(ticket);

			Assert.IsTrue(actual);
			var head = PoolResource.Head;
			Assert.IsNotNull(head);
			Assert.AreEqual(ticket.First, head.First);
			Assert.AreEqual(ticket.Last, head.Last);
			Assert.AreEqual(ticket.Count, head.Count);
		}

		[Test]
		public void SpamTest_0()
		{
			GLPoolResourceTicket ticket;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			{
				var actual = PoolResource.Free(ticket);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.AreEqual(ticket.First, head.First);
				Assert.AreEqual(ticket.Last, head.Last);
				Assert.AreEqual(ticket.Count, head.Count);
			}

			{
				var actual = PoolResource.Free(ticket);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.AreEqual(ticket.First, head.First);
				Assert.AreEqual(ticket.Last, head.Last);
				Assert.AreEqual(ticket.Count, head.Count);
			}
		}

		[Test]
		public void SpamTest_InsideRange_0()
		{
			GLPoolResourceTicket ticket_0;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket_0);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			{
				var actual = PoolResource.Free(ticket_0);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);
				Assert.AreEqual(ticket_0.First, head.First);
				Assert.AreEqual(ticket_0.Last, head.Last);
				Assert.AreEqual(ticket_0.Count, head.Count);
			}

			{
				var ticket_1 = new GLPoolResourceTicket
				{
					First = 0,
					Last = 1, 
					Count = 2,
				};

				var actual = PoolResource.Free(ticket_1);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);

				Assert.AreEqual(ticket_0.First, head.First);
				Assert.AreEqual(ticket_0.Last, head.Last);
				Assert.AreEqual(ticket_0.Count, head.Count);
			}
		}

		[Test]
		public void SpamTest_OutsideRange_0()
		{
			GLPoolResourceTicket ticket_0;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket_0);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			var ticket_1 = new GLPoolResourceTicket
			{
				First = 0,
				Last = 2,
				Count = 3,
			};

			{
				var actual = PoolResource.Free(ticket_1);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);
				Assert.AreEqual(ticket_1.First, head.First);
				Assert.AreEqual(ticket_1.Last, head.Last);
				Assert.AreEqual(ticket_1.Count, head.Count);
			}

			{
				var ticket_2 = new GLPoolResourceTicket
				{
					First = 0,
					Last = 3,
					Count = 4,
				};

				var actual = PoolResource.Free(ticket_2);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);

				Assert.AreEqual(ticket_2.First, head.First);
				Assert.AreEqual(ticket_2.Last, head.Last);
				Assert.AreEqual(ticket_2.Count, head.Count);
			}
		}

		[Test]
		public void SpamTest_OutsideRange_1()
		{
			GLPoolResourceTicket ticket_0;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket_0);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			var ticket_1 = new GLPoolResourceTicket
			{
				First = 0,
				Last = 1,
				Count = 2,
			};

			{
				var actual = PoolResource.Free(ticket_1);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);

				Assert.AreEqual(ticket_1.First, head.First);
				Assert.AreEqual(ticket_1.Last, head.Last);
				Assert.AreEqual(ticket_1.Count, head.Count);
			}

			{
				var ticket_2 = new GLPoolResourceTicket
				{
					First = 3,
					Last = 4,
					Count = 2,
				};

				var actual = PoolResource.Free(ticket_2);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);

				Assert.AreEqual(ticket_1.First, head.First);
				Assert.AreEqual(ticket_1.Last, head.Last);
				Assert.AreEqual(ticket_1.Count, head.Count);

				var next = head.Next;
				Assert.IsNotNull(next);

				Assert.AreEqual(ticket_2.First, next.First);
				Assert.AreEqual(ticket_2.Last, next.Last);
				Assert.AreEqual(ticket_2.Count, next.Count);
			}

			{
				var ticket_3 = new GLPoolResourceTicket
				{
					First = 0,
					Last = 4,
					Count = 5,
				};

				var actual = PoolResource.Free(ticket_3);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);

				Assert.AreEqual(ticket_3.First, head.First);
				Assert.AreEqual(ticket_3.Last, head.Last);
				Assert.AreEqual(ticket_3.Count, head.Count);

				var next = head.Next;
				Assert.IsNull(next);
			}
		}

		[Test]
		public void SpamTest_OutsideRange_2()
		{
			GLPoolResourceTicket ticket_0;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket_0);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			var ticket_1 = new GLPoolResourceTicket
			{
				First = 0,
				Last = 0,
				Count = 1,
			};

			{
				var actual = PoolResource.Free(ticket_1);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);

				Assert.AreEqual(ticket_1.First, head.First);
				Assert.AreEqual(ticket_1.Last, head.Last);
				Assert.AreEqual(ticket_1.Count, head.Count);
			}

			var ticket_2 = new GLPoolResourceTicket
			{
				First = 2,
				Last = 3,
				Count = 2,
			};

			{
				var actual = PoolResource.Free(ticket_2);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);

				Assert.AreEqual(ticket_1.First, head.First);
				Assert.AreEqual(ticket_1.Last, head.Last);
				Assert.AreEqual(ticket_1.Count, head.Count);

				var next_0 = head.Next;
				Assert.IsNotNull(next_0);

				Assert.AreEqual(ticket_2.First, next_0.First);
				Assert.AreEqual(ticket_2.Last, next_0.Last);
				Assert.AreEqual(ticket_2.Count, next_0.Count);
			}

			var ticket_3 = new GLPoolResourceTicket
			{
				First = 5,
				Last = 5,
				Count = 1,
			};

			{
				var actual = PoolResource.Free(ticket_3);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);

				Assert.AreEqual(ticket_1.First, head.First);
				Assert.AreEqual(ticket_1.Last, head.Last);
				Assert.AreEqual(ticket_1.Count, head.Count);

				var next_0 = head.Next;
				Assert.IsNotNull(next_0);

				Assert.AreEqual(ticket_2.First, next_0.First);
				Assert.AreEqual(ticket_2.Last, next_0.Last);
				Assert.AreEqual(ticket_2.Count, next_0.Count);

				var next_1 = next_0.Next;
				Assert.IsNotNull(next_1);

				Assert.AreEqual(ticket_3.First, next_1.First);
				Assert.AreEqual(ticket_3.Last, next_1.Last);
				Assert.AreEqual(ticket_3.Count, next_1.Count);
			}

			{
				var ticket_4 = new GLPoolResourceTicket
				{
					First = 0,
					Last = 2,
					Count = 3,
				};

				var actual = PoolResource.Free(ticket_4);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);

				Assert.AreEqual(0, head.First);
				Assert.AreEqual(3, head.Last);
				Assert.AreEqual(4, head.Count);

				var next = head.Next;
				Assert.IsNotNull(next);

				Assert.AreEqual(ticket_3.First, next.First);
				Assert.AreEqual(ticket_3.Last, next.Last);
				Assert.AreEqual(ticket_3.Count, next.Count);
			}
		}

		public void SpamTest_1()
		{
			GLPoolResourceTicket ticket;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			{
				var actual = PoolResource.Free(ticket);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.AreEqual(ticket.First, head.First);
				Assert.AreEqual(ticket.Last, head.Last);
				Assert.AreEqual(ticket.Count, head.Count);
			}

			{
				var actual = PoolResource.Free(ticket);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.AreEqual(ticket.First, head.First);
				Assert.AreEqual(ticket.Last, head.Last);
				Assert.AreEqual(ticket.Count, head.Count);
			}
		
			{
				var actual = PoolResource.Free(ticket);

				Assert.IsTrue(actual);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.AreEqual(ticket.First, head.First);
				Assert.AreEqual(ticket.Last, head.Last);
				Assert.AreEqual(ticket.Count, head.Count);
			}
		}

		[Test]
		public void TwoNodesAppended()
		{
			GLPoolResourceTicket ticket_0;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket_0);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			var ticket_1 = new GLPoolResourceTicket
			{
				First = 0,
				Last = 0,
				Count = 1,
			};

			var actual_1 = PoolResource.Free(ticket_1);

			Assert.IsTrue(actual_1);
			var head = PoolResource.Head;
			Assert.IsNotNull(head);
			Assert.IsNull(head.Next);

			Assert.AreEqual(ticket_1.First, head.First);
			Assert.AreEqual(ticket_1.Last, head.Last);
			Assert.AreEqual(ticket_1.Count, head.Count);

			var ticket_2 = new GLPoolResourceTicket
			{
				First = 2,
				Last = 2,
				Count = 1,
			};

			var actual_2 = PoolResource.Free(ticket_2);

			Assert.IsTrue(actual_2);
			head = PoolResource.Head;
			Assert.IsNotNull(head);

			var next = head.Next;
			Assert.IsNotNull(next);
			Assert.AreEqual(ticket_2.First, next.First);
			Assert.AreEqual(ticket_2.Last, next.Last);
			Assert.AreEqual(ticket_2.Count, next.Count);
		}

		[Test]
		public void LeftMerge()
		{
			GLPoolResourceTicket ticket_0;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket_0);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			var ticket_1 = new GLPoolResourceTicket
			{
				First = 0,
				Last = 0,
				Count = 1,
			};

			var actual_1 = PoolResource.Free(ticket_1);

			Assert.IsTrue(actual_1);
			var head = PoolResource.Head;
			Assert.IsNotNull(head);
			Assert.IsNull(head.Next);

			Assert.AreEqual(ticket_1.First, head.First);
			Assert.AreEqual(ticket_1.Last, head.Last);
			Assert.AreEqual(ticket_1.Count, head.Count);

			var ticket_2 = new GLPoolResourceTicket
			{
				First = 1,
				Last = 1,
				Count = 1,
			};

			var actual_2 = PoolResource.Free(ticket_2);

			Assert.IsTrue(actual_2);
			head = PoolResource.Head;
			Assert.IsNotNull(head);
			Assert.IsNull(head.Next);

			Assert.AreEqual(0, head.First);
			Assert.AreEqual(ticket_2.Last, head.Last);
			Assert.AreEqual(2, head.Count);
		}

		[Test]
		public void RightMerge()
		{
			GLPoolResourceTicket ticket_0;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket_0);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			{

				var ticket_1 = new GLPoolResourceTicket
				{
					First = 2,
					Last = 2,
					Count = 1,
				};

				var actual_1 = PoolResource.Free(ticket_1);

				Assert.IsTrue(actual_1);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);

				Assert.AreEqual(ticket_1.First, head.First);
				Assert.AreEqual(ticket_1.Last, head.Last);
				Assert.AreEqual(ticket_1.Count, head.Count);
			}

			{
				var ticket_2 = new GLPoolResourceTicket
				{
					First = 1,
					Last = 1,
					Count = 1,
				};

				var actual_2 = PoolResource.Free(ticket_2);

				Assert.IsTrue(actual_2);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);

				Assert.AreEqual(1, head.First);
				Assert.AreEqual(2, head.Last);
				Assert.AreEqual(2, head.Count);
			}
		}

		[Test]
		public void LeftAndRightMerge()
		{
			GLPoolResourceTicket ticket_0;
			var initialCheck = PoolResource.Allocate(NO_OF_ITEMS, out ticket_0);

			Assert.IsTrue(initialCheck);
			Assert.IsNull(PoolResource.Head);

			{
				var ticket_1 = new GLPoolResourceTicket
				{
					First = 0,
					Last = 0,
					Count = 1,
				};

				var actual_1 = PoolResource.Free(ticket_1);

				Assert.IsTrue(actual_1);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);

				Assert.AreEqual(ticket_1.First, head.First);
				Assert.AreEqual(ticket_1.Last, head.Last);
				Assert.AreEqual(ticket_1.Count, head.Count);
			}



			{
				var ticket_2 = new GLPoolResourceTicket
				{
					First = 2,
					Last = 2,
					Count = 1,
				};

				var actual_2 = PoolResource.Free(ticket_2);

				Assert.IsTrue(actual_2);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);

				var next = head.Next;
				Assert.IsNotNull(next);
				Assert.AreEqual(ticket_2.First, next.First);
				Assert.AreEqual(ticket_2.Last, next.Last);
				Assert.AreEqual(ticket_2.Count, next.Count);
			}


			{
				var ticket_3 = new GLPoolResourceTicket
				{
					First = 1,
					Last = 1,
					Count = 1,
				};

				var actual_3 = PoolResource.Free(ticket_3);

				Assert.IsTrue(actual_3);
				var head = PoolResource.Head;
				Assert.IsNotNull(head);
				Assert.IsNull(head.Next);
				Assert.AreEqual(0, head.First);
				Assert.AreEqual(2, head.Last);
				Assert.AreEqual(3, head.Count);
			}

		}
	}
}
