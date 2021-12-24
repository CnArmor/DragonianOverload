using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace Dragonian
{
    [HarmonyPatch(typeof(JobGiver_Manhunter),"TryGiveJob")]
    public class Patch_JobGiver_Manhunter
    {
        [HarmonyPostfix]
        private static void Postfix(ref Job __result, ref Pawn pawn)
        {
            //Log.Message("interferecing, pawn name:" + pawn.Name);
            //Log.Message("Ability CD: " + pawn.abilities.GetAbility(DragonianAbilityDefOf.Dragonian_DragonbloodOverload).CooldownTicksRemaining);
            if (pawn.IsDragonian() && pawn.Faction != Find.FactionManager.OfPlayer 
                && !pawn.health.hediffSet.HasHediff(DragonianHediffDefOf.Dragonian_DragonbloodOverload)
                && pawn.abilities.GetAbility(DragonianAbilityDefOf.Dragonian_DragonbloodOverload).CooldownTicksRemaining == 0)
                __result = FakeCastAbilityOnSelf(pawn);
        }

        private static Job FakeCastAbilityOnSelf(Pawn caster)
        {
            Job job = JobMaker.MakeJob(DragonianJobDefOf.DragonianFakeCast, caster);
            return job;
        }
    }

    [HarmonyPatch(typeof(JobGiver_AIFightEnemy), "TryGiveJob")]
    public class Patch_JobGiver_AIFightEnemy
    {
        [HarmonyPostfix]
        private static void Postfix(ref Job __result, ref Pawn pawn)
        {
            Thing enemyTarget = pawn.mindState.enemyTarget;
            Pawn targetPawn = enemyTarget as Pawn;
            if (enemyTarget != null && !targetPawn.IsInvisible() && pawn.IsDragonian() && pawn.Faction != Find.FactionManager.OfPlayer 
                && !pawn.health.hediffSet.HasHediff(DragonianHediffDefOf.Dragonian_DragonbloodOverload)
                && pawn.abilities.GetAbility(DragonianAbilityDefOf.Dragonian_DragonbloodOverload).CooldownTicksRemaining == 0)
                __result = FakeCastAbilityOnSelf(pawn);
        }
        private static Job FakeCastAbilityOnSelf(Pawn caster)
        {
            Job job = JobMaker.MakeJob(DragonianJobDefOf.DragonianFakeCast, caster);
            return job;
        }
    }
}
