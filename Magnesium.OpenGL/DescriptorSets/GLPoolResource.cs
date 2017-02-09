using System;
using System.Diagnostics;

namespace Magnesium.OpenGL.Internals
{
	public class GLPoolResource<T> : IGLDescriptorPoolResource<T>
	{
		public T[] Items { get; private set; }

		public GLPoolResource(uint count, T[] items)
		{
			Items = items;

			//if (count == 0)
			//	throw new ArgumentOutOfRangeException(nameof(count) + " must be greater than zero");

			Count = count;
			Head = new GLPoolResourceNode
			{
				First = 0,
				Last = (count > 0) ? (count - 1) : 0,
				Count = count,
				Next = null,
			};
		}

		public uint Count { get; private set; }

		public GLPoolResourceNode Head { get; private set; }

		public bool Allocate(uint request, out GLPoolResourceTicket range)
		{
			if (request == 0)
			{
				throw new ArgumentOutOfRangeException(nameof(request) + " must be greater than 0");
			}

			{
				// FIRST LOOP : SCAN FOR EXACT MATCHES
				GLPoolResourceNode current = Head;
				GLPoolResourceNode previous = null;
				while (current != null)
				{
					if (current.Count == request)
					{
						range = new GLPoolResourceTicket
						{
							First = current.First,
							Last = current.Last,
							Count = current.Count,
						};

						// remove current from linked list
						if (previous != null)
						{
							previous.Next = current.Next;
						}

						// remove from head
						if (ReferenceEquals(Head, current))
						{
							Head = current.Next;
						}

						return true;
					}
					previous = current;
					current = current.Next;
				}
			}

			{
				// SECOND LOOP : FIND FIRST BLOCK LARGE ENOUGH AND SPLIT 
				GLPoolResourceNode current = Head;
				while (current != null)
				{
					if (current.Count > request)
					{
						range = new GLPoolResourceTicket
						{
							First = current.First,
							Last = request + current.First - 1,
							Count = request,
						};

						// adjust current
						current.First += request;
						current.Count -= request;
						return true;
					}
					current = current.Next;
				}
			}

			// NOT FOUND
			range = null;
			return false;
		}

		public bool Free(GLPoolResourceTicket ticket)
		{
			if (Head == null)
			{
				Head = new GLPoolResourceNode
				{
					First = ticket.First,
					Last = ticket.Last,
					Count = ticket.Count,
				};
				return true;
			}
			else
			{
				GLPoolResourceNode previous = null;
				GLPoolResourceNode current = Head;

				while (current != null)
				{
					// SAME TICKET RANGE RECOVERY
					if (ticket.First == current.First)
					{
						// DO NOTHING IF INSIDE RANGE
						if (ticket.Last <= current.Last && ticket.Count <= current.Count)
						{
							return true;
						}
						else
						{
							AdjustTicketsSpan(ticket, current);
							return true;
						}
					}
					// GAP-BASED SLOT RECOVERY
					else if (ticket.First < current.First)
					{
						PerformMerge(previous, ticket, current);
						return true;
					}

					previous = current;
					current = current.Next;
				}

				// At end of linked list
				PerformMerge(previous, ticket, null);
				return true;
			}
		}

		void AdjustTicketsSpan(GLPoolResourceTicket ticket, GLPoolResourceNode parent)
		{
			GLPoolResourceNode lastNode = null;
			bool intercepts = false;

			GLPoolResourceNode current = parent.Next;
			while (current != null)
			{
				lastNode = current;
				if (current.Last > ticket.Last)
				{
					intercepts = (current.First <= ticket.Last);
					break;
				}

				// remove from the list 
				parent.Next = current.Next;
				current = current.Next;
			}

			parent.Last = (lastNode != null && intercepts) ? lastNode.Last : ticket.Last;
			parent.Count = parent.Last - parent.First + 1;

			if (current == null)
			{
				// ITERATED THRU THEN POINT TO END
				parent.Next = null;
			}
			else if (lastNode != null && intercepts)
			{
				// INCLUDE LAST NODE
				parent.Next = lastNode.Next;
			}
			else
			{
				// OUTSIDE OF LAST NODE
				parent.Next = lastNode;
			}

		}

		void PerformMerge(GLPoolResourceNode left, GLPoolResourceTicket ticket, GLPoolResourceNode right)
		{
			bool leftMerge = left != null && (ticket.First == (left.Last + 1));
			bool rightMerge = right != null && ((ticket.Last + 1) == right.First);

			ValidateTicket(ticket);
			ValidateLocation(left, ticket, right);

			if (leftMerge && rightMerge)
			{
				Debug.Assert(ReferenceEquals(left.Next, right));

				var finalCount = left.Count + ticket.Count + right.Count;
				Debug.Assert(finalCount == (right.Last - left.First + 1));

				left.Count = finalCount;
				left.Last = right.Last;
				left.Next = right.Next;
			}
			else if (leftMerge)
			{
				var finalCount = left.Count + ticket.Count;
				Debug.Assert(finalCount == (ticket.Last - left.First + 1));

				left.Count = finalCount;
				left.Last = ticket.Last;
			}
			else if (rightMerge)
			{
				var finalCount = right.Count + ticket.Count;
				Debug.Assert(finalCount == (right.Last - ticket.First + 1));

				right.Count = finalCount;
				right.First = ticket.First;
			}
			else
			{
				var inBetween = new GLPoolResourceNode
				{
					First = ticket.First,
					Last = ticket.Last,
					Count = ticket.Count,
				};

				// REPLACE HEAD
				if (left == null)
				{
					Debug.Assert(ReferenceEquals(right, Head));
					inBetween.Next = Head;
					Head = inBetween;
				}
				else
				{
					left.Next = inBetween;
					inBetween.Next = right;
				}
			}
		}

		void ValidateTicket(GLPoolResourceTicket ticket)
		{
			if ((ticket.First + ticket.Count) > Count) throw new InvalidOperationException();
			if (ticket.Count == 0) throw new InvalidOperationException();
			if ((ticket.First + ticket.Count - 1) != ticket.Last) throw new InvalidOperationException();
			if (ticket.Last > Count) throw new InvalidOperationException();
		}

		void ValidateLocation(GLPoolResourceNode previous, GLPoolResourceTicket ticket, GLPoolResourceNode current)
		{
			if (previous != null && previous.Last >= ticket.First) throw new InvalidOperationException();
			if (current != null && current.First <= ticket.Last) throw new InvalidOperationException();
		}
	}
}
