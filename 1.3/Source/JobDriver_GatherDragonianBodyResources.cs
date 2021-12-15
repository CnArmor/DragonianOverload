using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace Dragonian
{
    public abstract class JobDriver_GatherDragonianBodyResources : JobDriver
    {
        private float gatherProgress;

        protected const TargetIndex AnimalInd = TargetIndex.A;

        protected abstract float WorkTotal { get; }

        protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Pawn pawn = this.pawn;
            LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
            Job job = this.job;
            return ReservationUtility.Reserve(pawn, target, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            ToilFailConditions.FailOnDespawnedNullOrForbidden(this, TargetIndex.A);
            ToilFailConditions.FailOnNotCasualInterruptible(this, TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            Toil wait = new Toil();
            wait.initAction = delegate ()
            {
                Pawn actor = wait.actor;
                Pawn pawn = (Pawn)job.GetTarget(TargetIndex.A).Thing;
                actor.pather.StopDead();
                PawnUtility.ForceWait(pawn, 15000, null, true);
            };
            wait.tickAction = delegate ()
            {
                Pawn actor = wait.actor;
                actor.skills.Learn(SkillDefOf.Animals, 0.13f, false);
                gatherProgress += StatExtension.GetStatValue(actor, StatDefOf.AnimalGatherSpeed, true);
                if (gatherProgress >= WorkTotal)
                {
                    GetComp((Pawn)((Thing)job.GetTarget(TargetIndex.A))).Gathered(this.pawn);
                    actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
                }
            };
            wait.AddFinishAction(delegate ()
            {
                Pawn pawn = (Pawn)job.GetTarget(TargetIndex.A).Thing;
                if (pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
                {
                    pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
                }
            });
            ToilFailConditions.FailOnDespawnedOrNull<Toil>(wait, TargetIndex.A);
            ToilFailConditions.FailOnCannotTouch<Toil>(wait, TargetIndex.A, PathEndMode.Touch);
            wait.AddEndCondition(delegate ()
            {
                if (!GetComp((Pawn)((Thing)this.job.GetTarget(TargetIndex.A))).ActiveAndFull)
                {
                    return JobCondition.Incompletable;
                }
                return JobCondition.Ongoing;
            });
            wait.defaultCompleteMode = ToilCompleteMode.Never;
            ToilEffects.WithProgressBar(wait, TargetIndex.A, () => this.gatherProgress / this.WorkTotal, false, -0.5f);
            wait.activeSkill = (() => SkillDefOf.Animals);
            yield return wait;
            yield break;
        }
    }

    public class JobDriver_MilkDragonian : JobDriver_GatherDragonianBodyResources
    {
        protected override float WorkTotal
        {
            get
            {
                return 400f;
            }
        }

        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return animal.GetComp<CompMilkable>();
        }
    }

    public class JobDriver_ShearDragonian : JobDriver_GatherDragonianBodyResources
    {
        protected override float WorkTotal
        {
            get
            {
                return 400f;
            }
        }

        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return animal.GetComp<CompShearable>();
        }
    }
}
