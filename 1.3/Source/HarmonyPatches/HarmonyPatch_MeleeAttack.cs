using Verse;
using System.Linq;
using HarmonyLib;
using System;
using RimWorld;

namespace Dragonian
{
    [HarmonyPatch(typeof(VerbProperties), "AdjustedMeleeDamageAmount", new Type[] { typeof(Tool), typeof(Pawn), typeof(Thing), typeof(HediffComp_VerbGiver) })]
    public class Patch_VerbProperties_AdjustedEquipmentMeleeDamageAmount
    {
        //patches equipment melee damage
        [HarmonyPostfix]
        private static void VerbProperties_AdjustedEquipmentMeleeDamageAmountPostfix(ref float __result, Pawn attacker)
        {
            if(attacker != null)
            {
                if (attacker.GetStatValue(DragonianStatDefOf.DRO_MeleeDamageMultiplier) != 1f)
                {
                    __result *= attacker.GetStatValue(DragonianStatDefOf.DRO_MeleeDamageMultiplier);
                }
            }
        }
    }

    [HarmonyPatch(typeof(VerbProperties), "AdjustedMeleeDamageAmount", new Type[] { typeof(Tool), typeof(Pawn), typeof(ThingDef), typeof(ThingDef), typeof(HediffComp_VerbGiver) })]
    public class Patch_VerbProperties_AdjustedBarehandMeleeDamageAmount
    {
        //patches barehand melee damage
        [HarmonyPostfix]
        private static void VerbProperties_AdjustedBarehandMeleeDamageAmountPostfix(ref float __result, Pawn attacker)
        {
            if (attacker != null)
            {
                if (attacker.GetStatValue(DragonianStatDefOf.DRO_MeleeDamageMultiplier) != 1f)
                {
                    __result *= attacker.GetStatValue(DragonianStatDefOf.DRO_MeleeDamageMultiplier);
                }
            }
        }
    }

    [HarmonyPatch(typeof(VerbProperties), "AdjustedCooldown",new Type[] {typeof(Tool), typeof(Pawn), typeof(Thing)})]
    public class Patch_VerbProperties_AdjustedEquipmentMeleeCooldown
    {
        //patches equipment melee cooldown
        [HarmonyPostfix]
        private static void VerbProperties_AdjustedEquipmentMeleeCooldownPostfix(ref float __result, ref VerbProperties __instance, Pawn attacker)
        {
            if (attacker != null)
            {
                if (__instance.IsMeleeAttack && attacker.GetStatValue(DragonianStatDefOf.DRO_MeleeCooldownMultiplier) != 1f)
                {
                    __result *= attacker.GetStatValue(DragonianStatDefOf.DRO_MeleeCooldownMultiplier);
                }
            }
        }
    }

    [HarmonyPatch(typeof(VerbProperties), "AdjustedCooldown", new Type[] { typeof(Tool), typeof(Pawn), typeof(ThingDef), typeof(ThingDef) })]
    public class Patch_VerbProperties_AdjustedBarehandMeleeCooldown
    {
        //patches bare hand melee cooldown
        [HarmonyPostfix]
        private static void VerbProperties_AdjustedBarehandMeleeCooldownPostfix(ref float __result, ref VerbProperties __instance, Pawn attacker)
        {
            if (attacker != null)
            {
                if (__instance.IsMeleeAttack && attacker.GetStatValue(DragonianStatDefOf.DRO_MeleeCooldownMultiplier) != 1f)
                {
                    __result *= attacker.GetStatValue(DragonianStatDefOf.DRO_MeleeCooldownMultiplier);
                }
            }
        }
    }
}