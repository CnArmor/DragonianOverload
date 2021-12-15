using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace Dragonian
{
    public class JobDriver_SelfMilkDragonian : JobDriver
    {
        private float gatherProgress;

        private float workTotal = 600f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
        }
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Toil wait = new Toil();

            wait.activeSkill = (() => SkillDefOf.Animals);
            wait.defaultCompleteMode = ToilCompleteMode.Never;
            wait.initAction = delegate ()
            {
                wait.actor.pather.StopDead();
            };
            wait.tickAction = delegate ()
            {
                Pawn actor = wait.actor;
                actor.skills.Learn(SkillDefOf.Animals, 0.13f, false);
                gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
                if(gatherProgress >= workTotal)
                {
                    actor.GetComp<CompMilkable>().Gathered(actor);
                    actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
                }
            };
            wait.FailOnDespawnedOrNull(TargetIndex.A);
            wait.FailOnDownedOrDead(TargetIndex.A);
            wait.FailOnNotCasualInterruptible(TargetIndex.A);
            wait.WithProgressBar(TargetIndex.A, () => gatherProgress / workTotal, false, -0.5f);
            
            yield return wait;

            yield break;
        }
    }
}
