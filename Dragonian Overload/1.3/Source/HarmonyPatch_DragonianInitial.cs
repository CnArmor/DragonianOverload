using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace Dragonian
{

    [HarmonyPatch(typeof(PawnGenerator), "TryGenerateNewPawnInternal")]
    public static class Patch_PawnGenerator
    {
        [HarmonyPostfix]
        static void Postfix(ref Pawn __result)
        {
            if (__result != null)
            {
                if (__result?.def == DragonianRaceDefOf.Dragonian_Female)
                {
                    __result.abilities?.GainAbility(DragonianAbilityDefOf.Dragonian_DragonbloodOverload);
                    if (!__result.health.hediffSet.hediffs.Any(hd => hd.def == DragonianHediffDefOf.Dragonian_AutoRecovery))
                        __result.health.AddHediff(DragonianHediffDefOf.Dragonian_AutoRecovery);
                }
            }
        }
    }

    // For backward compatiblity
    [HarmonyPatch(typeof(Pawn_AbilityTracker), "ExposeData")]
    public static class Patch_Pawn_AbilityTracker
    {
        [HarmonyPostfix]
        static void Postfix(ref Pawn_AbilityTracker __instance, ref Pawn ___pawn)
        {
            if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
            {
                if (___pawn?.def == DragonianRaceDefOf.Dragonian_Female && !___pawn.abilities.abilities.Any(ab => ab.def == DragonianAbilityDefOf.Dragonian_DragonbloodOverload))
                {
                    __instance.GainAbility(DragonianAbilityDefOf.Dragonian_DragonbloodOverload);
                }
            }
        }
    }
}