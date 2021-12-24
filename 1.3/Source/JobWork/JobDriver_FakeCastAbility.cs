using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace Dragonian
{
    public class JobDriver_DragonianFakeCastAbility : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
            Toil cast = new Toil();
            Ability dragonianOverload = pawn.abilities.GetAbility(DragonianAbilityDefOf.Dragonian_DragonbloodOverload);

            cast.FailOnDespawnedOrNull(TargetIndex.A);

            if (dragonianOverload.CooldownTicksRemaining == 0)
            {
                cast.initAction = delegate ()
                {
                    pawn.pather.StopDead();
                    pawn.health.AddHediff(DragonianHediffDefOf.Dragonian_DragonbloodOverload);
                    dragonianOverload.StartCooldown(dragonianOverload.def.cooldownTicksRange.RandomInRange);
                };
                cast.defaultCompleteMode = ToilCompleteMode.Delay;
                cast.defaultDuration = (int)(dragonianOverload.def.verbProperties.warmupTime * 60);
                cast.PlaySoundAtStart(DragonianSoundDefOf.Dragonian_Ability);
            }
            else
            {
                cast.defaultCompleteMode = ToilCompleteMode.Instant;
            }
            yield return cast;

            yield break;
        } 
    }
}