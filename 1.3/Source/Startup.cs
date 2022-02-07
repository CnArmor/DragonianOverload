using System.Reflection;
using RimWorld;
using Verse;
using HarmonyLib;
using System;
using Verse.AI;

namespace Dragonian
{
    [StaticConstructorOnStartup]

    public static class HarmonyPatches
    {
        //must have for Harmony to actually patch things
        private static readonly Type patchType = typeof(HarmonyPatches);
        static HarmonyPatches()
        {
            Harmony harmony = new Harmony("Dragonian");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            harmony.Patch(AccessTools.Method(typeof(InteractionWorker_RecruitAttempt), "DoRecruit", new[] { typeof(Pawn), typeof(Pawn), typeof(string).MakeByRefType(), typeof(string).MakeByRefType(), typeof(bool), typeof(bool) }),
                postfix: new HarmonyMethod(patchType, nameof(Patch_InteractionWorker_RecruitAttempt)));
        }
        public static void Patch_InteractionWorker_RecruitAttempt(ref Pawn recruitee) //on successfully recruiting wild dragonians, change the pawnkind so they no longer have wildman behaviours
        {
            if (recruitee.IsWildDragonian())
            {
                //Log.Message(recruitee.Name+"");
                recruitee.ClearMind();
                recruitee.kindDef = DragonianPawnKindDefOf.Dragonian_Civil;
            }
        }
    }

    [DefOf]
    public class DragonianRaceDefOf
    {
        public static ThingDef Dragonian_Female;
    }
 
    [DefOf]
    public class DragonianBodyDefOf
    {
        public static BodyDef Dragonian;
    }

    [DefOf]
    public class DragonianPawnKindDefOf
    {
        public static PawnKindDef Dragonian_Female;
        public static PawnKindDef Dragonian_Slave_Female;
        public static PawnKindDef Dragonian_Wild;
        public static PawnKindDef Dragonian_Civil;
    }

    [DefOf]
    public class DragonianAbilityDefOf
    {
        public static AbilityDef Dragonian_DragonbloodOverload;
        public static AbilityDef Dragonian_PoweredStaggerImmunity;
    }

    [DefOf]
    public class DragonianHediffDefOf
    {
        public static HediffDef Dragonian_AutoRecovery;
        public static HediffDef Dragonian_DragonbloodBurn;
        public static HediffDef Dragonian_DragonbloodOverload;
    }

    [DefOf]
    public class DragonianDamageDefOf
    {
        public static DamageDef Dragonian_DragonbloodBurn;
    }

    [DefOf]
    public class DragonianJobDefOf
    {
        public static JobDef MilkingDragonian;
        public static JobDef ShearingDragonian;
        public static JobDef SelfMilkingDragonian;
        public static JobDef DragonianFakeCast;
    }

    [DefOf]
    public class DragonianEffecterDefOf
    {
        public static EffecterDef Dragonian_Effecter_Blush_north;
        public static EffecterDef Dragonian_Effecter_Blush_south;
        public static EffecterDef Dragonian_Effecter_Blush_east;
        public static EffecterDef Dragonian_Effecter_Blush_west;
        public static EffecterDef Dragonian_Effecter_Sweat;
    }

    [DefOf]
    public class DragonianFactionDefOf
    {
        public static FactionDef Dragonians_Hidden;
    }

    [DefOf]
    public class DragonianDutyDefOf
    {
        public static DutyDef Dragonian_WildWanderNearPoint;
    }

    [DefOf]
    public class DragonianThingDefOf
    {
        public static ThingDef DragonianMilk;
        public static ThingDef WoolDragonian;
    }

    [DefOf]
    public class DragonianSoundDefOf
    {
        public static SoundDef Dragonian_Ability;
        public static SoundDef Dragonian_Shutdown;
    }

    [DefOf]
    public class DragonianStatDefOf
    {
        public static StatDef DRO_PowerMax;
        public static StatDef DRO_PowerRechargeRate;
        public static StatDef DRO_MeleeDamageMultiplier;
        public static StatDef DRO_MeleeCooldownMultiplier;
    }

    [HarmonyPatch(typeof(ForbidUtility))]
    [HarmonyPatch("SetForbidden")]
    public static class FobidPatch
    {

        [HarmonyPrefix]
        static bool Prefix(this Thing t, bool value, bool warnOnFail = true)
        {
            if (t == null)
            {
                if (warnOnFail)
                {
                    Log.Error("Tried to SetForbidden on null Thing.");
                }
                return false;
            }
            ThingWithComps thingWithComps = t as ThingWithComps;
            if (thingWithComps == null)
            {
                if (warnOnFail)
                {
                    Log.Error("Tried to SetForbidden on non-ThingWithComps Thing " + t);
                }
                return false;
            }
            CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
            if (comp == null)
            {
                return false;
            }
            comp.Forbidden = value;
            return false;
        }

    }
}