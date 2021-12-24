using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Dragonian
{
	public class TransitionAction_GoMad : TransitionAction
	{
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
			}
		}
	}
	public class TransitionAction_OutOfManhunter : TransitionAction
	{
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				pawn.ClearMind();
			}
		}
	}
}