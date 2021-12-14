using Verse;
using System.Linq;
using HarmonyLib;
using System;

namespace Dragonian
{
    [HarmonyPatch(typeof(VerbProperties), "AdjustedMeleeDamageAmount", new Type[] { typeof(Tool), typeof(Pawn), typeof(Thing), typeof(HediffComp_VerbGiver) })]
    public class Patch_VerbProperties_AdjustedEquipmentMeleeDamageAmount
    {
        //patches equipment melee damage
        [HarmonyPostfix]
        private static void Postfix(ref float __result, Pawn attacker)
        {
            if (attacker.def == DragonianRaceDefOf.Dragonian_Female
                && attacker.health.hediffSet.hediffs.Any(hd => hd.def == DragonianHediffDefOf.Dragonian_DragonbloodOverload))
            {
                __result = __result * DragonianHediffDefOf.Dragonian_DragonbloodOverload.GetModExtension<MeleeModifier>().meleeDamageMultiplier;
            }
        }
    }

    [HarmonyPatch(typeof(VerbProperties), "AdjustedMeleeDamageAmount", new Type[] { typeof(Tool), typeof(Pawn), typeof(ThingDef), typeof(ThingDef), typeof(HediffComp_VerbGiver) })]
    public class Patch_VerbProperties_AdjustedBarehandMeleeDamageAmount
    {
        //patches barehand melee damage
        [HarmonyPostfix]
        private static void Postfix(ref float __result, Pawn attacker)
        {
            if (attacker.def == DragonianRaceDefOf.Dragonian_Female
                && attacker.health.hediffSet.hediffs.Any(hd => hd.def == DragonianHediffDefOf.Dragonian_DragonbloodOverload))
            {
                __result = __result * DragonianHediffDefOf.Dragonian_DragonbloodOverload.GetModExtension<MeleeModifier>().meleeDamageMultiplier;
            }
        }
    }

    [HarmonyPatch(typeof(VerbProperties), "AdjustedCooldown",new Type[] {typeof(Tool), typeof(Pawn), typeof(Thing)})]
    public class Patch_VerbProperties_AdjustedEquipmentMeleeCooldown
    {
        //patches equipment melee cooldown
        [HarmonyPostfix]
        private static void Postfix(ref float __result, ref VerbProperties __instance, Pawn attacker)
        {
            if (__instance.IsMeleeAttack && attacker.def == DragonianRaceDefOf.Dragonian_Female 
                && attacker.health.hediffSet.hediffs.Any(hd => hd.def == DragonianHediffDefOf.Dragonian_DragonbloodOverload))
            {
                __result = __result * DragonianHediffDefOf.Dragonian_DragonbloodOverload.GetModExtension<MeleeModifier>().meleeCooldownMultiplier;
            }
        }
    }

    [HarmonyPatch(typeof(VerbProperties), "AdjustedCooldown", new Type[] { typeof(Tool), typeof(Pawn), typeof(ThingDef), typeof(ThingDef) })]
    public class Patch_VerbProperties_AdjustedBarehandMeleeCooldown
    {
        [HarmonyPostfix]
        //patches bare hand melee cooldown
        private static void Postfix(ref float __result, ref VerbProperties __instance, Pawn attacker)
        {
            if (__instance.IsMeleeAttack && attacker.def == DragonianRaceDefOf.Dragonian_Female
                && attacker.health.hediffSet.hediffs.Any(hd => hd.def == DragonianHediffDefOf.Dragonian_DragonbloodOverload))
            {
                __result = __result * DragonianHediffDefOf.Dragonian_DragonbloodOverload.GetModExtension<MeleeModifier>().meleeCooldownMultiplier;
            }
        }
    }
}