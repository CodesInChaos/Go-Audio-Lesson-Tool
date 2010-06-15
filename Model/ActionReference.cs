using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public struct ActionReference
	{
		public Replay Replay { get; private set; }
		public int Index { get; private set; }
		public GameAction Action
		{
			get
			{
				if (IsValid)
					return Replay.Actions[Index];
				else
					throw new InvalidOperationException("Cannot dereference invalid ActionReference");
			}
		}

		public ActionReference Previous
		{
			get
			{
				return new ActionReference(Replay, Index - 1);
			}
		}

		public ActionReference Next
		{
			get
			{
				return new ActionReference(Replay, Index + 1);
			}
		}

		public bool IsValid
		{
			get
			{
				return Replay != null && Index >= 0 && Index < Replay.Actions.Count;
			}
		}

		public static ActionReference operator --(ActionReference ar)
		{
			return ar.Previous;
		}

		public static ActionReference operator ++(ActionReference ar)
		{
			return ar.Next;
		}

		public ActionReference(Replay replay, int index)
			: this()
		{
			if (replay == null)
				throw new ArgumentNullException("replay");
			Replay = replay;
			Index = index;
		}

		public static bool operator ==(ActionReference ref1, ActionReference ref2)
		{
			return (ref1.Replay == ref2.Replay) && (ref1.Index == ref2.Index);
		}

		public static bool operator !=(ActionReference ref1, ActionReference ref2)
		{
			return !(ref1 == ref2);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			return (this == (ActionReference)obj);
		}

		public override int GetHashCode()
		{
			return Replay.GetHashCode() ^ Index;
		}
	}
}
