using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Dragonian
{
    public class JobGiver_ComeBackNearDutyLocation : ThinkNode_JobGiver
    {
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ComeBackNearDutyLocation jobGiver_ComeBackNearDutyLocation = (JobGiver_ComeBackNearDutyLocation)base.DeepCopy(resolve);
			jobGiver_ComeBackNearDutyLocation.wanderRadius = this.wanderRadius;
			jobGiver_ComeBackNearDutyLocation.tooFarRadius = this.tooFarRadius;
			jobGiver_ComeBackNearDutyLocation.wanderDestValidator = this.wanderDestValidator;
			jobGiver_ComeBackNearDutyLocation.locomotionUrgency = this.locomotionUrgency;
			jobGiver_ComeBackNearDutyLocation.locomotionUrgencyTooFar = this.locomotionUrgencyTooFar;
			jobGiver_ComeBackNearDutyLocation.maxDanger = this.maxDanger;
			jobGiver_ComeBackNearDutyLocation.expiryInterval = this.expiryInterval;
			return jobGiver_ComeBackNearDutyLocation;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 wanderRoot = GetDutyPosition(pawn);
			IntVec3 wanderDest = GetWanderDest(pawn);
			if (pawn.CurJob == null && !pawn.Position.InHorDistOf(wanderRoot, this.wanderRadius)) 
			{
				Job job = JobMaker.MakeJob(JobDefOf.GotoWander, wanderDest);
				if (!pawn.Position.InHorDistOf(wanderRoot, this.tooFarRadius))
					job.locomotionUrgency = this.locomotionUrgencyTooFar;
				else
					job.locomotionUrgency = this.locomotionUrgency;
				job.expiryInterval = this.expiryInterval;
				return job;
			}
			return null;
		}

		protected IntVec3 GetDutyPosition(Pawn pawn)
        {
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
        }

		protected virtual IntVec3 GetWanderDest(Pawn pawn)
        {
			float radius = this.wanderRadius;
			if (pawn.mindState.duty != null && pawn.mindState.duty.wanderRadius != null)
				radius = pawn.mindState.duty.wanderRadius.Value;
			return RCellFinder.RandomWanderDestFor(pawn, GetDutyPosition(pawn), radius, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, maxDanger));

		}
		protected float wanderRadius = 6f;

		protected float tooFarRadius = 20f;

		protected Func<Pawn, IntVec3, IntVec3, bool> wanderDestValidator;

		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		protected LocomotionUrgency locomotionUrgencyTooFar = LocomotionUrgency.Jog;

		protected Danger maxDanger = Danger.None;

		protected int expiryInterval = -1;
	}
}